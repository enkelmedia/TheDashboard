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

namespace TheDashboard.Api
{

    [IsBackOffice]
    [CamelCaseController]
    public class TheDashboardController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public DashboardViewModel GetViewModel()
        {

            var repo = new UmbracoRepository();

            var listUnpublishedContent = repo.GetUnpublishedConent();
            var logItems = repo.GetLatestLogItems();
            var nodesInRecyleBin = repo.GetRecycleBinNodes().Select(x => x.Id).ToArray();

            var dashboardViewModel = new DashboardViewModel();

            foreach (var logItem in logItems.Take(10))
            {
            
                var user = GetUser(logItem.UserId);
                var contentNode = GetContent(logItem.NodeId);
             
                if (contentNode == null || !CurrentUserHasPermissions(contentNode))
                    continue;

                var vm = new ActivityViewModel();
                vm.UserDisplayName = user.Name;
                vm.UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user);
                vm.NodeId = logItem.NodeId;
                vm.NodeName = contentNode.Name;
                vm.Message = logItem.Comment;
                vm.LogItemType = logItem.LogType.ToString();
                vm.Timestamp = logItem.Timestamp;

                var unpublishedVersionOfLogItem = listUnpublishedContent.FirstOrDefault(x => x.NodeId == logItem.NodeId && x.ReleaseDate != null);
                if (logItem.LogType == LogTypes.Save && unpublishedVersionOfLogItem != null && unpublishedVersionOfLogItem.UpdateDate != null)
                {
                    if(logItem.Timestamp.IsSameMinuteAs(unpublishedVersionOfLogItem.UpdateDate.Value))
                    vm.LogItemType = "SavedAndScheduled";
                    vm.ScheduledPublishDate = unpublishedVersionOfLogItem.ReleaseDate;
                }

                if (logItem.LogType == LogTypes.UnPublish && nodesInRecyleBin.Contains(logItem.NodeId))
                {
                    vm.LogItemType = "UnPublishToRecycleBin";
                }

                dashboardViewModel.Activities.Add(vm);
                
            }

            
            foreach (var item in listUnpublishedContent.Where(x => x.ReleaseDate == null))
            {
                
                var user = GetUser(item.DocumentUser);
                var contentNode = GetContent(item.NodeId);

                // Checking for null, makring sure that user has permissions and checking for this content node in the recycle bin. If its in the recycle bin
                // we should no return this as an unpublished node.
                if (contentNode == null || !CurrentUserHasPermissions(contentNode) || nodesInRecyleBin.Contains(contentNode.Id))
                    continue;

                var vm = new ActivityViewModel();
                vm.UserDisplayName = user.Name;
                vm.UserAvatarUrl = UserAvatarProvider.GetAvatarUrl(user);
                vm.NodeId = item.NodeId;
                vm.NodeName = contentNode.Name;
                vm.Timestamp = item.UpdateDate.Value;


                dashboardViewModel.UnpublishedContent.Add(vm);

            }

            
            foreach (var item in logItems.Where(x => x.UserId == Security.CurrentUser.Id).Take(10))
            {
                var contentNode = GetContent(item.NodeId);

                if (contentNode == null || !CurrentUserHasPermissions(contentNode))
                    continue;

                var vm = new ActivityViewModel();
                vm.NodeId = item.NodeId;
                vm.NodeName = contentNode.Name;
                vm.LogItemType = item.LogType.ToString();
                vm.Timestamp = item.Timestamp;

                dashboardViewModel.UserRecentActivity.Add(vm);

            }

            dashboardViewModel.CountPublishedNodes = repo.CountPublishedNodes();
            dashboardViewModel.CountTotalWebsiteMembers = repo.CountMembers();
            dashboardViewModel.CountNewMembersLastWeek = repo.CountNewMember();
            dashboardViewModel.CountContentInRecycleBin = nodesInRecyleBin.Count();

            return dashboardViewModel;
        }

        private bool CurrentUserHasPermissions(IContent contentNode)
        {
            return global::umbraco.BusinessLogic.Actions.Action.FromString(UmbracoUser.GetPermissions(contentNode.Path)).OfType<ActionBrowse>().Any();
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