using Our.Umbraco.TheDashboard.Counters.Implement;
using Our.Umbraco.TheDashboard.Extensions;
using Our.Umbraco.TheDashboard.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.TheDashboard.Controllers.OpenApi;
using Our.Umbraco.TheDashboard.Counters.Collections;
using Umbraco.Cms.Api.Common.OpenApi;

namespace Our.Umbraco.TheDashboard
{
    public class TheDashboardComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {

            builder.Services.AddTransient<ITheDashboardService, TheDashboardService>();

            builder.WithCollectionBuilder<DashboardCountersCollectionBuilder>()
                .Append<ContentTotalContentItemsDashboardCounter>()
                .Append<ContentInRecycleBinDashboardCounter>()
                .Append<MembersTotalDashboardCounter>();

            // Just using this to make sure that it works and are used in the package
            builder.TheDashboardCounters().Append<MembersNewLastWeekDashboardCounter>();

#if DEBUG
            // SWAGGER - Only use in debug build to avoid exposing in production messing up things in the core.
            builder.Services.ConfigureOptions<ConfigureTheDashboardApiSwaggerGenOptions>();
            builder.Services.AddSingleton<ISchemaIdHandler, TheDashboardSchemaIdHandler>();
#endif

            
        }

    }
}
