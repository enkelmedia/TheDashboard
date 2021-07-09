using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.TheDashboard.Counters.Collections;
using Our.Umbraco.TheDashboard.Mapping;
using Our.Umbraco.TheDashboard.Models.Dtos;
using Our.Umbraco.TheDashboard.Models.Frontend;
using Our.Umbraco.TheDashboard.Security;
using Our.Umbraco.TheDashboard.Services;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;


namespace Our.Umbraco.TheDashboard.Controllers
{
    [IsBackOffice]
    [JsonCamelCaseFormatter]
    public class TheDashboardController : UmbracoAuthorizedJsonController
    {
        private readonly AppCaches _appCaches;
        private readonly IScopeProvider _scopeProvider;
        private readonly ITheDashboardService _dashboardService;
        private readonly DashboardCountersCollection _dashboardCountersCollection;
        private readonly IUserService _userService;
        private readonly IBackOfficeSecurity _security;
        private readonly IEntityService _entityService;

        public TheDashboardController(AppCaches appCaches, 
            IScopeProvider scopeProvider,
            ITheDashboardService dashboardService, 
            DashboardCountersCollection dashboardCountersCollection,
            IUserService userService,
            IBackOfficeSecurity security,
            IEntityService entityService
        )
        {
            _appCaches = appCaches;
            _scopeProvider = scopeProvider;
            _dashboardService = dashboardService;
            _dashboardCountersCollection = dashboardCountersCollection;
            _userService = userService;
            _security = security;
            _entityService = entityService;
        }

        [HttpGet]
        public RecentActivitiesFrontendModel GetAllRecentActivities()
        {
            
            var model = new RecentActivitiesFrontendModel();
            model.AllItems = new List<RecentActivityFrontendModel>();
            model.YourItems = new List<RecentActivityFrontendModel>();

            var allRecentDtos = _dashboardService.GetEntries();

            var accessService = new UserToNodeAccessHelper(_security.CurrentUser, _userService,_entityService,_appCaches, allRecentDtos);
            var filteredDtos = allRecentDtos.Where(x => accessService.HasAccessTo(x)).ToList();

            model.AllItems = CreateFrontendModelsFrom(filteredDtos);

            // filter to only my own
            var filteredMyActivitiesDtos = filteredDtos.Where(x => x.UserId == _security.CurrentUser.Id).ToList();
            model.YourItems = CreateFrontendModelsFrom(filteredMyActivitiesDtos);

            return model;
        }

        [HttpGet]
        public UnpublishedContentNotScheduledFrontendModel GetUnpublished()
        {
            var model = new UnpublishedContentNotScheduledFrontendModel();
            model.Items = new List<RecentActivityFrontendModel>();
            
            var allRecentDtos = _dashboardService.GetUnpublished();

            var accessService = new UserToNodeAccessHelper(_security.CurrentUser, _userService,_entityService,_appCaches, allRecentDtos);
            var filteredDtos = allRecentDtos.Where(x => accessService.HasAccessTo(x)).ToList();

            model.Items = CreateFrontendModelsFrom(filteredDtos);
            model.Count = model.Items.Count;
            return model;
        }


        private List<RecentActivityFrontendModel> CreateFrontendModelsFrom(List<LogEntryDto> dtos)
        {
            var maxCount = 10;
            var mapper = new LogEntryToRecentActivityMapper(_appCaches);

            // Should return a list of models containing unique items for the nodeId.
            var list = new List<RecentActivityFrontendModel>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                // If we already have a item in the list with the same nodeId this means that the latest activity
                // for this nodeid already has been added.
                if (!list.Any(x => x.NodeId == dto.NodeId))
                {
                    // try to create a view model, this will return null when the model is not of a valid type.
                    var vm = mapper.Map(dto);
                    if (vm != null)
                    {
                        list.Add(vm);
                    }
                }

                if(list.Count >= maxCount)
                    return list;

            }

            return list;

        }
        


        public string GetMyRecentActivities()
        {
            return "";
        }

        public CountersFrontendModel GetCounters()
        {
            var model = new CountersFrontendModel();

            using (var scope = _scopeProvider.CreateScope())
            {
                foreach (var counter in _dashboardCountersCollection)
                {
                    model.Counters.Add(counter.GetModel(scope));
                }
            }

            return model;
        }




        //[HttpGet]
        //public DashboardViewModel GetViewModel()
        //{
        //    var dashboardViewModel = new DashboardViewModel();

        //    var umbracoRepository = new UmbracoRepository();

        //    var unpublishedContent = umbracoRepository.GetUnpublishedContent().ToArray();
        //    var logItems = umbracoRepository.GetLatestLogItems().ToArray();
        //    var nodesInRecyleBin = umbracoRepository.GetRecycleBinNodes().Select(x => x.Id).ToArray();

        //    var topTenLogItems = logItems.Take(10).ToList();
        //    var unpublishedContentWhichHasNeverBeenPublished = unpublishedContent.Where(x => x.ReleaseDate == null).ToList();
        //    var topTenLogItemsForCurrentUser = logItems.Where(x => x.UserId == Security.CurrentUser.Id).Take(10).ToList();

        //    var nodeIds = topTenLogItems.Select(x => x.NodeId).ToList();
        //    nodeIds.AddRange(unpublishedContentWhichHasNeverBeenPublished.Select(x => x.NodeId));
        //    nodeIds.AddRange(topTenLogItemsForCurrentUser.Select(x => x.NodeId));
            
        //    var NodeIdPermissions = ApplicationContext.Services.UserService.GetPermissions(Security.CurrentUser, nodeIds.ToArray());

        //    foreach (var logItem in logItems.Take(10))
        //    {
        //        if (!CurrentUserHasPermissions(NodeIdPermissions, logItem.NodeId))
        //            continue;


        //        var user = GetUser(logItem.UserId);

        //        var activityViewModel = new ActivityViewModel
        //        {
        //            UserDisplayName = user.Name,
        //            UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
        //            NodeId = logItem.NodeId,
        //            NodeName = GetContentNodeName(logItem.NodeId),
        //            Message = logItem.Comment,
        //            LogItemType = logItem.LogType.ToString(),
        //            Timestamp = logItem.Timestamp
        //        };

        //        var unpublishedVersionOfLogItem = unpublishedContent.FirstOrDefault(x => x.NodeId == logItem.NodeId && x.ReleaseDate != null);
        //        if (logItem.LogType == LogTypes.Save && unpublishedVersionOfLogItem != null && unpublishedVersionOfLogItem.UpdateDate != null)
        //        {
        //            if (logItem.Timestamp.IsSameMinuteAs(unpublishedVersionOfLogItem.UpdateDate.Value))
        //                activityViewModel.LogItemType = "SavedAndScheduled";
        //            activityViewModel.ScheduledPublishDate = unpublishedVersionOfLogItem.ReleaseDate;
        //        }

        //        if (logItem.LogType == LogTypes.UnPublish && nodesInRecyleBin.Contains(logItem.NodeId))
        //        {
        //            activityViewModel.LogItemType = "UnPublishToRecycleBin";
        //        }

        //        dashboardViewModel.Activities.Add(activityViewModel);
        //    }


        //    foreach (var item in unpublishedContentWhichHasNeverBeenPublished)
        //    {
        //        if (!CurrentUserHasPermissions(NodeIdPermissions, item.NodeId))
        //        {
        //            continue;
        //        }
        //        if (nodesInRecyleBin.Contains(item.NodeId))
        //        {
        //            continue;
        //        }
        //        // Checking for null, making sure that user has permissions and checking for this content node in the recycle bin. If its in the recycle bin
        //        // we should not return this as an unpublished node.

        //        var user = GetUser(item.DocumentUser);
        //        var activityViewModel = new ActivityViewModel
        //        {
        //            UserDisplayName = user.Name,
        //            UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user),
        //            NodeId = item.NodeId,
        //            NodeName = GetContentNodeName(item.NodeId),
        //            Timestamp = item.UpdateDate != null ? item.UpdateDate.Value : (DateTime?)null
        //        };

        //        dashboardViewModel.UnpublishedContent.Add(activityViewModel);
        //    }

        //    foreach (var item in topTenLogItemsForCurrentUser)
        //    {
        //        if (!CurrentUserHasPermissions(NodeIdPermissions, item.NodeId))
        //        {
        //            continue;
        //        }


        //        var activityViewModel = new ActivityViewModel
        //        {
        //            NodeId = item.NodeId,
        //            NodeName = GetContentNodeName(item.NodeId),
        //            LogItemType = item.LogType.ToString(),
        //            Timestamp = item.Timestamp
        //        };

        //        dashboardViewModel.UserRecentActivity.Add(activityViewModel);
        //    }




        //    dashboardViewModel.CountPublishedNodes = umbracoRepository.CountPublishedNodes();

        //    dashboardViewModel.CountTotalWebsiteMembers = umbracoRepository.CountMembers();

        //    dashboardViewModel.CountNewMembersLastWeek = umbracoRepository.CountNewMember();

        //    dashboardViewModel.CountContentInRecycleBin = nodesInRecyleBin.Count();

        //    return dashboardViewModel;
        //}

        //private string GetContentNodeName(int nodeId)
        //{
        //    //first check in Examine as this is WAY faster
        //    var criteria = ExamineManager.Instance
        //        .SearchProviderCollection["InternalSearcher"]
        //        .CreateSearchCriteria("content");
        //    var filter = criteria.Id(nodeId);
        //    var results = ExamineManager
        //        .Instance.SearchProviderCollection["InternalSearcher"]
        //        .Search(filter.Compile());
        //    if (results.Any())
        //    {
        //        var firstResult = results.First();
        //        return firstResult.Fields["nodeName"];
        //    }
        //    else
        //    {
        //        var contentNode = Services.ContentService.GetById(nodeId);
        //        if (contentNode != null)
        //        {
        //            return contentNode.Name;
        //        }
        //    }
        //    return null;
        //}

        //private bool CurrentUserHasPermissions(EntityPermissionCollection permissionResults, int nodeId)
        //{
        //    //This is done for us in 7.7.2
        //    var perms = permissionResults.Where(x => x.EntityId == nodeId).SelectMany(x => x.AssignedPermissions).Distinct().ToArray();
        //    return perms.Any(x => x == "F");
        //}

        //private IUser GetUser(int id)
        //{
        //    return Services.UserService.GetByProviderKey(id);
        //}

    }

    
}