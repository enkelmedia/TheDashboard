using Umbraco.Cms.Core.Composing;

namespace Our.Umbraco.TheDashboard.Counters.Collections;

public class DashboardCountersCollectionBuilder : OrderedCollectionBuilderBase<DashboardCountersCollectionBuilder, DashboardCountersCollection, IDashboardCounter>
{
    protected override DashboardCountersCollectionBuilder This => this;
}
