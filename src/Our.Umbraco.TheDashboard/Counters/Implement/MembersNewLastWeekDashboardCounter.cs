using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.TheDashboard.Counters.Implement;

public class MembersNewLastWeekDashboardCounter : IDashboardCounter
{
    public MembersNewLastWeekDashboardCounter()
    {
    }

    public DashboardCounterModel GetModel(IScope scope)
    {
        var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember
                   inner join umbracoNode on cmsMember.nodeId = umbraconode.id     
                    WHERE createDate > @0", DateTime.Now.AddDays(-7));

        return new DashboardCounterModel()
        {
            LocalizationKey = "theDashboard_newMembersLastWeek",
            Count = count,
            ClickUrl = "section/member-management",
            Style = DashboardCounterModel.CounterStyles.Selected
        };
    }
}
