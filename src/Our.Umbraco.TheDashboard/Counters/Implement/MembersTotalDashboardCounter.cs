using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.TheDashboard.Counters.Implement;

public class MembersTotalDashboardCounter : IDashboardCounter
{
    public MembersTotalDashboardCounter()
    {
    }

    public DashboardCounterModel GetModel(IScope scope)
    {
        var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember");
            
        return new DashboardCounterModel()
        {
            LocalizationKey = "theDashboard_membersOnWebsite",
            Count = count,
            ClickUrl = "section/member-management/workspace/member-root",
            Style = DashboardCounterModel.CounterStyles.Selected
        };
    }
}
