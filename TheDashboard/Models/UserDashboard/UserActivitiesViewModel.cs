using System;

namespace TheDashboard.Models
{
    public class UserActivitiesViewModel
    {
        public UserActivitiesViewModel(DateTime datestamp) 
        {
            Datestamp = "";
            if (datestamp != null) 
            {
                this.Datestamp = datestamp.ToString("yyyy-MM-dd HH:mm");
                this.RealDate = datestamp;
            }
        }
        public int NodeId { get; set; }
        public string Datestamp { get; set; }
        public DateTime RealDate { get; set; }
        public string LogHeader { get; set; }
        public string LogComment { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public string UserTypeAlias { get; set; }


    }
}