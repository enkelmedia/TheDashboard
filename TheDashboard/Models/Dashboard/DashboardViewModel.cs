using System.Collections.Generic;

namespace TheDashboard.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Activities= new List<ActivityViewModel>();
            UnpublishedContent = new List<ActivityViewModel>();
            UserRecentActivity = new List<ActivityViewModel>();
        }

        public List<ActivityViewModel> Activities { get; set; }
        public List<ActivityViewModel> UnpublishedContent { get; set; }
        public List<ActivityViewModel> UserRecentActivity { get; set; }

        public int CountPublishedNodes { get; set; }
        public int CountContentInRecycleBin { get; set; }
        public int CountTotalWebsiteMembers { get; set; }
        public int CountNewMembersLastWeek { get; set; }
    }
}