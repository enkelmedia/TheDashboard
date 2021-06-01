using Our.Umbraco.TheDashboard.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.TheDashboard.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations.Upgrade.V_7_12_0;
using Umbraco.Core.Models;
using Umbraco.Web.Actions;

namespace Our.Umbraco.TheDashboard.Security
{
    internal class UserToNodeAccessHelper
    {
        private EntityPermissionCollection _permissions;

        /// <summary>
        /// Stores the users start nodes
        /// </summary>
        private int[] _userStartNodes;

        internal UserToNodeAccessHelper(IUser currentUser, IUserService userService, IEnumerable<IUmbracoNodeWithPermissions> nodes)
        {
            _userStartNodes = currentUser.CalculateContentStartNodeIds(Current.Services.EntityService);
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
            if (_userStartNodes.Length > 1)
            {
                // Users with access to "all" will have a startNode with -1
                if (_userStartNodes.Length > 1 || _userStartNodes[0] != Constants.System.Root)
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
}
