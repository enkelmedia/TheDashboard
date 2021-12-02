using Our.Umbraco.TheDashboard.Counters;
using Our.Umbraco.TheDashboard.Counters.Collections;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.TheDashboard.Extensions
{
    public static class CompositionExtensions
    {
        
            /// <summary>
            /// Used to modify the collection of Dashboard Counters for The Dashboard
            /// </summary>
            /// <param name="builder"></param>
            /// <returns></returns>
            public static DashboardCountersCollectionBuilder TheDashboardCounters(this IUmbracoBuilder builder)
                => builder.WithCollectionBuilder<DashboardCountersCollectionBuilder>();


            /// <summary>
            /// Used to remove a Dashboard Counter for The Dashboard
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="builder"></param>
            /// <returns></returns>
            public static IUmbracoBuilder RemoveCounterDashboard<T>(this IUmbracoBuilder builder) 
                where T : class, IDashboardCounter
            {
                builder.TheDashboardCounters().Remove<T>();
                return builder;
            }
        
    }
}
