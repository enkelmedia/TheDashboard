using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

namespace Our.Umbraco.TheDashboard.Counters.Collections
{
    
    public class DashboardCountersCollection : BuilderCollectionBase<IDashboardCounter>
    {
        public DashboardCountersCollection(IEnumerable<IDashboardCounter> counters) : base(counters)
        { }
    }
}