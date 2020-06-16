using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Our.Umbraco.TheDashboard.Counters.Collections;
using Umbraco.Core.Composing;

namespace Our.Umbraco.TheDashboard.Extensions
{
    public static class CompositionExtensions
    {
        
            /// <summary>
            /// Used to modify the collection of Dashboard Counters for The Dashboard
            /// </summary>
            /// <param name="composition"></param>
            /// <returns></returns>
            public static DashboardCountersCollectionBuilder TheDashboardCounters(this Composition composition)
                => composition.WithCollectionBuilder<DashboardCountersCollectionBuilder>();
        
    }
}
