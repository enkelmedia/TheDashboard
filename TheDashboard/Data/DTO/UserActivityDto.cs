namespace TheDashboard.Data.DTO
{
    using System;

    public class UserActivityDto
    {
        public int NodeId { get; set; }
        public DateTime Datestamp { get; set; }
        public string LogHeader { get; set; }
        public string LogComment { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public string UserTypeAlias { get; set; }

       
    }
}