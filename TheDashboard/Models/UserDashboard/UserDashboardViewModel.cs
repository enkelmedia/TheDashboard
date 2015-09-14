using System.Collections.Generic;

namespace TheDashboard.Models
{
    public class UserDashboardViewModel
    {
        public UserDashboardViewModel() 
        {
            UsersActivitiesLog = new List<UserActivitiesViewModel>(); 
        }

        public List<UserActivitiesViewModel> UsersActivitiesLog { get; set; }
        
    }

    
}


    