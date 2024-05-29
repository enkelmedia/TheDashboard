using Our.Umbraco.TheDashboard.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.TheDashboard.Security;

internal class UserToNodeAccessHelper
{
    private readonly IEntityService _entityService;
    private readonly AppCaches _appCaches;
    private EntityPermissionCollection _permissions;

    /// <summary>
    /// Stores the users start nodes
    /// </summary>
    private int[] _userStartNodes;

    internal UserToNodeAccessHelper(
        IUser currentUser, 
        IUserService userService, 
        IEntityService entityService,
        AppCaches appCaches,
        IEnumerable<IUmbracoNodeWithPermissions> nodes)
    {
        _entityService = entityService;
        _appCaches = appCaches;
        _userStartNodes = currentUser.CalculateContentStartNodeIds(_entityService,appCaches).Distinct().ToArray();
        _permissions = userService.GetPermissions(currentUser, AllNodeIdsFromPath(nodes));
    }

    private int[] AllNodeIdsFromPath(IEnumerable<IUmbracoNodeWithPermissions> nodes)
    {
        List<int> ids = new List<int>();
            
        foreach (var node in nodes)
        {
            var nodeIds = node.NodePath.Split(',').Select(x=>int.Parse(x));
            ids.AddRange(nodeIds);
        }

        return ids.Distinct().ToArray();

    }

    public bool HasAccessTo(IUmbracoNodeWithPermissions node)
    {
        // Here we'll traverse the tree from the current node up up children of root.
        // Since the EntityPermissionCollection returned from UserService.GetPermissions might Grant "Browse"-permissions
        // for a given node but Denied for one of it's parents.

        // Parse path and reverse to start from the current node.
        var nodeIds = node.NodePath
                .Split(',')
                .Reverse()
                .Select(x => int.Parse(x))
                .Where(x => x > 0) // exlude -1 as this is the root and has no permissions.
                .ToList()
            ;

        // Compare usersStart node(s) (if any) against the path, if not found in start nodes access should be denied
        if (_userStartNodes.Length > 0)
        {

            bool onlyRootInList = _userStartNodes.Length == 1 && _userStartNodes[0] == Constants.System.Root;

            // Users with access to "all" will have a startNode with -1
            if (_userStartNodes.Length > 1 || !onlyRootInList)
            {
                // At this stage we know that the user has start nodes assigned.

                // Compare ids in the path against ids in _startNodes.
                // At lest one of the ids in _userStartNodes must be present in the path
                bool found = false;

                foreach (var startNodeId in _userStartNodes)
                {
                    var inPath = nodeIds.Contains(startNodeId);
                    if (inPath)
                    {
                        found = true;
                        break;
                    }
                }

                // If none of the start node ids was found in the path we should 
                // not allow access.
                if(!found)
                    return false;

            }

        }

        // Look for permissions on the current node and it's anscestors.
        foreach (var id in nodeIds)
        {
            if(!HasPermissions(_permissions,id))
                return false;
        }

        // No denied permissions, access to node is granted.
        return true;
    }

    private bool HasPermissions(EntityPermissionCollection permissionResults, int nodeId)
    {
        var forNode = permissionResults.Where(x => x.EntityId == nodeId).ToList();
        var permissions = forNode.SelectMany(x => x.AssignedPermissions).ToList();

        // Getting the letter for the "Browse"-permissions (probably "F"), using Umbraco.Web/Actions/ActionBrowse.cs
        var browseNodeAction = ActionBrowse.ActionLetter.ToString();

        return permissions.Contains(browseNodeAction);
    }

}
