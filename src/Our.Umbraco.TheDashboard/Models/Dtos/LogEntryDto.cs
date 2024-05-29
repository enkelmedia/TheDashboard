namespace Our.Umbraco.TheDashboard.Models.Dtos;

public class LogEntryDto : IUmbracoNodeWithPermissions
{
    public int UserId { get; set; }
    public int NodeId { get; set; }
    public Guid NodeKey { get; set; }
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

