using System;

namespace TheDashboard.Data.DTO
{
    public class UnpublishedNode
    {
        public Int32 NodeId { get; set; }
        public string Text { get; set; }
        public int DocumentUser { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}