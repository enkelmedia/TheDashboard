using Our.Umbraco.TheDashboard.Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.TheDashboard.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace Our.Umbraco.TheDashboard.Security
{
    internal class UserToNodeAccessHelper
    {
        private EntityPermissionCollection _permissions;

        internal UserToNodeAccessHelper(IUser currentUser, IUserService userService, IEnumerable<IUmbracoNodeWithPermissions> nodes)
        {
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
            var nodeIds = node.NodePath.Split(',').Reverse().Select(x=>int.Parse(x));

            // Loop over each ancestor and return false if permissions are denied.
            foreach (var id in nodeIds)
            {
                if(id > 0 && !HasPermissions(_permissions,id))
                    return false;
            }

            // No denied permissions, access to node is granted.
            return true;
        }

        private bool HasPermissions(EntityPermissionCollection permissionResults, int nodeId)
        {
            var forNode = permissionResults.Where(x => x.EntityId == nodeId).ToList();
            var permissions = forNode.SelectMany(x => x.AssignedPermissions).ToList();

            return permissions.Contains("F");
        }

    }
}
