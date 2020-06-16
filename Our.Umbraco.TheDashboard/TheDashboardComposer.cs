using Our.Umbraco.TheDashboard.Counters.Implement;
using Our.Umbraco.TheDashboard.Extensions;
using Our.Umbraco.TheDashboard.Services;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace Our.Umbraco.TheDashboard
{
    public class TheDashboardComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<TheDashboardComponent>();
            composition.Dashboards().Add<TheDashboardDashboard>();

            composition.Register<ITheDashboardService,TheDashboardService>();


            composition.TheDashboardCounters().Append<ContentTotalContentItemsDashboardCounter>();
            composition.TheDashboardCounters().Append<ContentInRecycleBinDashboardCounter>();
            composition.TheDashboardCounters().Append<MembersTotalDashboardCounter>();
            composition.TheDashboardCounters().Append<MembersNewLastWeekDashboardCounter>();


        }
    }
}