using Our.Umbraco.TheDashboard.Counters.Implement;
using Our.Umbraco.TheDashboard.Extensions;
using Our.Umbraco.TheDashboard.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.TheDashboard.Counters.Collections;

namespace Our.Umbraco.TheDashboard
{
    public class TheDashboardComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {

            builder.Dashboards().Add<TheDashboardDashboard>();
            builder.Services.AddTransient<ITheDashboardService, TheDashboardService>();

            builder.WithCollectionBuilder<DashboardCountersCollectionBuilder>()
                .Append<ContentTotalContentItemsDashboardCounter>()
                .Append<ContentInRecycleBinDashboardCounter>()
                .Append<MembersTotalDashboardCounter>();

            // Just using this to make sure that it works and are used in the package
            builder.TheDashboardCounters().Append<MembersNewLastWeekDashboardCounter>();

        }
    }
}