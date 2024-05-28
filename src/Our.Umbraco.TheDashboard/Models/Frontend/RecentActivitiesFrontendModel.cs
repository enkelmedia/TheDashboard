using System.Collections.Generic;

namespace Our.Umbraco.TheDashboard.Models.Frontend
{
    public class RecentActivitiesFrontendModel
    {
        public List<RecentActivityFrontendModel> AllItems { get; set; }
        public List<RecentActivityFrontendModel> YourItems { get; set; }

    }
}