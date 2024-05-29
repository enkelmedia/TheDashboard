namespace Our.Umbraco.TheDashboard.Models;

public interface IUmbracoNodeWithPermissions
{
    int NodeId { get; set; }
    string NodePath { get; set; }
}
