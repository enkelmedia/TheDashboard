using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.TheDashboard
{
    public static class TheDashboardConstants
    {
        public static class ActivityTypes
        {
            public const string Publish = "Publish";
            public const string Save = "Save";
            public const string SaveAndScheduled = "SaveAndScheduled";
            public const string RollBack = "RollBack";
            public const string RecycleBin = "RecycleBin";
            public const string Unpublish = "Unpublish";
        }

        public static class UmbracoLogHeaders
        {
            public const string Publish = "Publish";
            public const string Save = "Save";
            
            public const string RollBack = "RollBack";
            public const string Move = "Move";
            public const string Delete = "Delete";
            public const string Unpublish = "Unpublish";


            public const string Copy = "Copy";
            public const string Sort = "Sort";
        }
        
    }
}
