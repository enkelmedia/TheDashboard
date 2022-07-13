using System;
using NPoco;

namespace Our.Umbraco.TheDashboard.Models.Dtos
{
    public class LogEntryDto : IUmbracoNodeWithPermissions
    {
        public int UserId { get; set; }
        public int NodeId { get; set; }
        public string EntityType { get; set; }
        public DateTime Datestamp { get; set; }

        public string LogHeader { get; set; }
        public string LogComment { get; set; }

        public string NodeName { get; set; }
        public string NodePath { get; set; }
        public DateTime? NodeScheduledDate { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserAvatar { get; set; }

    }

    public class UmbracoNodeDTO
    {
        [Column("id")]
        public int Id {get;set;}

        [Column("nodeObjectType")]
        public string ObjectType { get; internal set; }

        [Column("trashed")]
        public bool Trashed { get; internal set; }
    }
}