using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Our.Umbraco.TheDashboard.Extensions;
using Our.Umbraco.TheDashboard.Models.Dtos;
using Our.Umbraco.TheDashboard.Models.Frontend;
using Umbraco.Core.Cache;

namespace Our.Umbraco.TheDashboard.Mapping
{
    public class LogEntryToRecentActivityMapper
    {
        private readonly AppCaches _appCaches;

        public LogEntryToRecentActivityMapper(AppCaches appCaches)
        {
            _appCaches = appCaches;
        }

        public RecentActivityFrontendModel Map(LogEntryDto dto)
        {
            var dashboardLogEntryType = GetLogEntryType(dto);

            if (string.IsNullOrEmpty(dashboardLogEntryType))
                return null;

            return new RecentActivityFrontendModel()
            {
                NodeId = dto.NodeId,
                NodeName = dto.NodeName,
                Datestamp = dto.Datestamp,
                ScheduledPublishDate = dto.NodeScheduledDate,
                ActivityType = dashboardLogEntryType,
                User = new UserFrontendModel() {
                    Name = dto.UserName,
                    Avatar = UserExtensions.GetUserAvatarUrls(dto.UserId,dto.UserEmail,dto.UserAvatar, _appCaches.RuntimeCache)
                }
            };

        }

        private string GetLogEntryType(LogEntryDto dto)
        {
            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.Publish)
                return TheDashboardConstants.ActivityTypes.Publish;

            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.Save && dto.NodeScheduledDate.HasValue)
                return TheDashboardConstants.ActivityTypes.SaveAndScheduled;

            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.Save)
                return TheDashboardConstants.ActivityTypes.Save;

            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.Unpublish)
                return TheDashboardConstants.ActivityTypes.Unpublish;

            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.RollBack)
                return TheDashboardConstants.ActivityTypes.RollBack;

            if (dto.LogHeader == TheDashboardConstants.UmbracoLogHeaders.Delete && dto.LogComment.Contains("Trashed"))
                return TheDashboardConstants.ActivityTypes.RecycleBin;

            // Empty string means this is a type of LogEntryDto that we don't care about.
            return string.Empty;

        }

    }
}
