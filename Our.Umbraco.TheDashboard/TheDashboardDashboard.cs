using System;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace Our.Umbraco.TheDashboard
{
    
    [Weight(-10000)]
    public class TheDashboardDashboard : IDashboard
    {
        public string Alias => "Our.Umbraco.TheDashboard";
        public string View => "/App_Plugins/Our.Umbraco.TheDashboard/TheDashboard.html";

        public string[] Sections => new[]
        {
            global::Umbraco.Cms.Core.Constants.Applications.Content
        };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();
    }
}
