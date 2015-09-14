using System;
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
#pragma warning disable 612,618
using Action = umbraco.BusinessLogic.Actions.Action;
using Umbraco.Core.Logging;
using System.Collections.Generic;
#pragma warning restore 612,618

namespace TheDashboard.Api
{
    [IsBackOffice]
    [CamelCaseController]
    public class TheUserDashboardController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public UserDashboardViewModel GetActivitiesByDate(DateTime date)
        {
            var umbracoRepository = new UmbracoRepository();
            var userDashboardViewModel = new UserDashboardViewModel();

            //if this date then get first date
            if (date.ToString("yyyy-MM-dd") == "1987-05-10")
            {
                var userActivitiesDate = umbracoRepository.GetAvailableDates();
                date = userActivitiesDate.ToArray()[0];
            }


            var userActivities = umbracoRepository.GetUserActivities(date).ToArray();

            foreach (var userActivityItem in userActivities)
            {

                var useractViewObj = new UserActivitiesViewModel(userActivityItem.Datestamp)
                    {
                        NodeId = userActivityItem.NodeId,
                        LogComment = userActivityItem.LogComment,
                        LogHeader = userActivityItem.LogHeader,
                        Text = userActivityItem.Text,
                        Username = userActivityItem.Username,
                        UserTypeAlias = userActivityItem.UserTypeAlias
                    };

                userDashboardViewModel.UsersActivitiesLog.Add(useractViewObj);


            }
            return userDashboardViewModel;
        }
        [HttpGet]
        public IEnumerable<string> GetAvailableDates()
        {
            var umbracoRepository = new UmbracoRepository();
            var userDashboardViewModel = new UserDashboardViewModel();

            var userActivitiesDate = umbracoRepository.GetAvailableDates().ToArray();

            IList<string> itemTransform = new List<string>();

            foreach (DateTime date in userActivitiesDate)
            {

                itemTransform.Add(date.ToString("yyyy-MM-dd"));
            }


            return itemTransform;
        }
        /**[HttpPost]
        public UserDashboardViewModel SearchActivities([FromBody] string text)
        {
            var umbracoRepository = new UmbracoRepository();
            var userDashboardViewModel = new UserDashboardViewModel();



            var userActivities = umbracoRepository.GetUserActivitiesSearch(text).ToArray();

            foreach (Data.DTO.UserActivityDto userActivityItem in userActivities)
            {
                //string username = "";
                //string textData = "";
                //string logComment = "";
                //if (!String.IsNullOrEmpty(userActivityItem.Username)) {
                //    username = userActivityItem.Username.ToUpper();
                //}
                //if (!String.IsNullOrEmpty(userActivityItem.Text))
                //{
                //    textData = userActivityItem.Text.ToUpper();
                //}
                //if (!String.IsNullOrEmpty(userActivityItem.LogComment))
                //{
                //    logComment = userActivityItem.LogComment.ToUpper();
                //}
                //if (username.Contains(toUpperSearch) || textData.Contains(toUpperSearch) || logComment.Contains(toUpperSearch))
                //{
                var useractViewObj = new UserActivitiesViewModel(userActivityItem.Datestamp)
                {
                    NodeId = userActivityItem.NodeId,
                    LogComment = userActivityItem.LogComment,
                    LogHeader = userActivityItem.LogHeader,
                    Text = userActivityItem.Text,
                    Username = userActivityItem.Username,
                    UserTypeAlias = userActivityItem.UserTypeAlias
                };

                userDashboardViewModel.UsersActivitiesLog.Add(useractViewObj);
                //}


            }
            return userDashboardViewModel;
        }**/

        private bool CurrentUserHasPermissions(IContent contentNode)
        {
            return Action.FromString(UmbracoUser.GetPermissions(contentNode.Path)).OfType<ActionBrowse>().Any();
        }
    }
}