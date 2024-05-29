using System;
using Our.Umbraco.TheDashboard.Controllers;

namespace Our.Umbraco.TheDashboard.Models.Frontend
{
    public class RecentActivityFrontendModel
    {
        public string ActivityType { get; set; }

        public string NodeName { get; set; }
        
        public Guid NodeKey { get; set; }

        public UserFrontendModel User { get; set; }
        public DateTime Datestamp { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
    }
}
