using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.TheDashboard.Models
{
    public interface IUmbracoNodeWithPermissions
    {
        int NodeId { get; set; }
        string NodePath { get; set; }
    }
}

