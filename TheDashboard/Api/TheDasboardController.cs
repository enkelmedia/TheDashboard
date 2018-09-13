using Examine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using TheDashboard.Api.Attributes;
using TheDashboard.Core;
using TheDashboard.Core.Extensions;
using TheDashboard.Data;
using TheDashboard.Models;
using umbraco.BusinessLogic;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
#pragma warning disable 612, 618
using Action = umbraco.BusinessLogic.Actions.Action;
#pragma warning restore 612, 618

namespace TheDashboard.Api
{
	[IsBackOffice]
	[CamelCaseController]
	public class TheDashboardController : UmbracoAuthorizedJsonController
	{
		[HttpGet]
		public DashboardViewModel GetViewModel()
		{
			var dashboardViewModel = new DashboardViewModel();

			var umbracoRepository = new UmbracoRepository();

			var unpublishedContent = umbracoRepository.GetUnpublishedContent().ToArray();
			var logItems = umbracoRepository.GetLatestLogItems().ToArray();
			var nodesInRecyleBin = umbracoRepository.GetRecycleBinNodes().Select(x => x.Id).ToArray();

			var topTenLogItems = logItems.Take(10).ToList();
			var unpublishedContentWhichHasNeverBeenPublished = unpublishedContent.Where(x => x.ReleaseDate == null).ToList();
			var topTenLogItemsForCurrentUser = logItems.Where(x => x.UserId == Security.CurrentUser.Id).Take(10).ToList();

			var nodeIds = topTenLogItems.Select(x => x.NodeId).ToList();
			nodeIds.AddRange(unpublishedContentWhichHasNeverBeenPublished.Select(x => x.NodeId));
			nodeIds.AddRange(topTenLogItemsForCurrentUser.Select(x => x.NodeId));

			var NodeIdPermissions = ApplicationContext.Services.UserService.GetPermissions(Security.CurrentUser, nodeIds.ToArray());


			foreach (var logItem in logItems.Take(10))
			{
				if (!CurrentUserHasPermissions(NodeIdPermissions, logItem.NodeId))
					continue;


				var user = GetUser(logItem.UserId);

				var activityViewModel = new ActivityViewModel
				{
					UserDisplayName = user.Name,
					UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
					NodeId = logItem.NodeId,
					NodeName = GetContentNodeName(logItem.NodeId),
					Message = logItem.Comment,
					LogItemType = logItem.LogType.ToString(),
					Timestamp = logItem.Timestamp
				};

				var unpublishedVersionOfLogItem = unpublishedContent.FirstOrDefault(x => x.NodeId == logItem.NodeId && x.ReleaseDate != null);
				if (logItem.LogType == LogTypes.Save && unpublishedVersionOfLogItem != null && unpublishedVersionOfLogItem.UpdateDate != null)
				{
					if (logItem.Timestamp.IsSameMinuteAs(unpublishedVersionOfLogItem.UpdateDate.Value))
						activityViewModel.LogItemType = "SavedAndScheduled";
					activityViewModel.ScheduledPublishDate = unpublishedVersionOfLogItem.ReleaseDate;
				}

				if (logItem.LogType == LogTypes.UnPublish && nodesInRecyleBin.Contains(logItem.NodeId))
				{
					activityViewModel.LogItemType = "UnPublishToRecycleBin";
				}

				dashboardViewModel.Activities.Add(activityViewModel);
			}


			foreach (var item in unpublishedContentWhichHasNeverBeenPublished)
			{
				if (!CurrentUserHasPermissions(NodeIdPermissions, item.NodeId))
				{
					continue;
				}
				if (nodesInRecyleBin.Contains(item.NodeId))
				{
					continue;
				}
				// Checking for null, making sure that user has permissions and checking for this content node in the recycle bin. If its in the recycle bin
				// we should not return this as an unpublished node.

				var user = GetUser(item.DocumentUser);
				var activityViewModel = new ActivityViewModel
				{
					UserDisplayName = user.Name,
					UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
					NodeId = item.NodeId,
					NodeName = GetContentNodeName(item.NodeId),
					Timestamp = item.UpdateDate != null ? item.UpdateDate.Value : (DateTime?)null
				};

				dashboardViewModel.UnpublishedContent.Add(activityViewModel);
			}

			foreach (var item in topTenLogItemsForCurrentUser)
			{
				if (!CurrentUserHasPermissions(NodeIdPermissions, item.NodeId))
				{
					continue;
				}


				var activityViewModel = new ActivityViewModel
				{
					NodeId = item.NodeId,
					NodeName = GetContentNodeName(item.NodeId),
					LogItemType = item.LogType.ToString(),
					Timestamp = item.Timestamp
				};

				dashboardViewModel.UserRecentActivity.Add(activityViewModel);
			}

			dashboardViewModel.CountPublishedNodes = umbracoRepository.CountPublishedNodes();

			dashboardViewModel.CountTotalWebsiteMembers = umbracoRepository.CountMembers();

			dashboardViewModel.CountNewMembersLastWeek = umbracoRepository.CountNewMember();

			dashboardViewModel.CountContentInRecycleBin = nodesInRecyleBin.Count();

			return dashboardViewModel;
		}

		private string GetContentNodeName(int nodeId)
		{
			//first check in Examine as this is WAY faster
			var criteria = ExamineManager.Instance
				.SearchProviderCollection["InternalSearcher"]
				.CreateSearchCriteria("content");
			var filter = criteria.Id(nodeId);
			var results = ExamineManager
				.Instance.SearchProviderCollection["InternalSearcher"]
				.Search(filter.Compile());
			if (results.Any())
			{
				var firstResult = results.First();
				return firstResult.Fields["nodeName"];
			}
			else
			{
				var contentNode = Services.ContentService.GetById(nodeId);
				if (contentNode != null)
				{
					return contentNode.Name;
				}
			}
			return null;
		}

		private bool CurrentUserHasPermissions(IEnumerable<EntityPermission> permissionResults, int nodeId)
		{
			var perms = permissionResults.Where(x => x.EntityId == nodeId).SelectMany(x => x.AssignedPermissions).Distinct().ToArray();
			var actionBrowseLetter = ActionBrowse.Instance.Letter.ToString();
			return perms.Any(x => x == actionBrowseLetter);
		}

		private IUser GetUser(int id)
		{
			return Services.UserService.GetByProviderKey(id);
		}


	}
}