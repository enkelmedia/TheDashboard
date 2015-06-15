using System;
using System.Linq;
using System.Web.Http;
using TheDashboard.Api.Attributes;
using TheDashboard.Core;
using TheDashboard.Core.Extentions;
using TheDashboard.Data;
using TheDashboard.Models;
using umbraco.BusinessLogic;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;
#pragma warning disable 612,618
using Action = umbraco.BusinessLogic.Actions.Action;
#pragma warning restore 612,618

namespace TheDashboard.Api
{
    [IsBackOffice]
    [CamelCaseController]
    public class TheDashboardController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public DashboardViewModel GetViewModel()
        {
            var umbracoRepository = new UmbracoRepository();
            var dashboardViewModel = new DashboardViewModel();

            var unpublishedContent = umbracoRepository.GetUnpublishedContent().Take(10).ToArray();
            var logItems = umbracoRepository.GetLatestLogItems().ToArray();
            var nodesInRecyleBin = umbracoRepository.GetRecycleBinNodes().Select(x => x.Id).ToArray();

            foreach (var logItem in logItems.Take(10))
            {
                var user = GetUser(logItem.UserId);
                var contentNode = GetContent(logItem.NodeId);
             
                if (contentNode == null || !CurrentUserHasPermissions(contentNode))
                    continue;

                var activityViewModel = new ActivityViewModel
                    {
                        UserDisplayName = user.Name,
                        UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
                        NodeId = logItem.NodeId,
                        NodeName = contentNode.Name,
                        Message = logItem.Comment,
                        LogItemType = logItem.LogType.ToString(),
                        Timestamp = logItem.Timestamp
                    };

                var unpublishedVersionOfLogItem = unpublishedContent.FirstOrDefault(x => x.NodeId == logItem.NodeId && x.ReleaseDate != null);
                if (logItem.LogType == LogTypes.Save && unpublishedVersionOfLogItem != null && unpublishedVersionOfLogItem.UpdateDate != null)
                {
                    if(logItem.Timestamp.IsSameMinuteAs(unpublishedVersionOfLogItem.UpdateDate.Value))
                    activityViewModel.LogItemType = "SavedAndScheduled";
                    activityViewModel.ScheduledPublishDate = unpublishedVersionOfLogItem.ReleaseDate;
                }

                if (logItem.LogType == LogTypes.UnPublish && nodesInRecyleBin.Contains(logItem.NodeId))
                {
                    activityViewModel.LogItemType = "UnPublishToRecycleBin";
                }

                dashboardViewModel.Activities.Add(activityViewModel);
            }

            foreach (var item in unpublishedContent.Where(x => x.ReleaseDate == null))
            {
                var user = GetUser(item.DocumentUser);
                var contentNode = GetContent(item.NodeId);

                // Checking for null, makring sure that user has permissions and checking for this content node in the recycle bin. If its in the recycle bin
                // we should no return this as an unpublished node.
                if (contentNode == null || !CurrentUserHasPermissions(contentNode) || nodesInRecyleBin.Contains(contentNode.Id))
                    continue;

                var activityViewModel = new ActivityViewModel
                    {
                        UserDisplayName = user.Name,
                        UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
                        NodeId = item.NodeId,
                        NodeName = contentNode.Name,
                        Timestamp = item.UpdateDate != null ? item.UpdateDate.Value : (DateTime?) null
                    };

                dashboardViewModel.UnpublishedContent.Add(activityViewModel);
            }

            foreach (var item in logItems.Where(x => x.UserId == Security.CurrentUser.Id).Take(10))
            {
                var contentNode = GetContent(item.NodeId);

                if (contentNode == null || !CurrentUserHasPermissions(contentNode))
                    continue;

                var activityViewModel = new ActivityViewModel
                    {
                        NodeId = item.NodeId,
                        NodeName = contentNode.Name,
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

        private bool CurrentUserHasPermissions(IContent contentNode)
        {
            return Action.FromString(UmbracoUser.GetPermissions(contentNode.Path)).OfType<ActionBrowse>().Any();
        }

        private IUser GetUser(int id)
        {
            return Services.UserService.GetByProviderKey(id);
        }

        private IContent GetContent(int id)
        {
            var doc = Services.ContentService.GetById(id);
            return doc;
        }
    }
}