using System;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace Our.Umbraco.TheDashboard.Counters.Implement
{
    public class MembersNewLastWeekDashboardCounter : IDashboardCounter
    {
        private readonly ILocalizedTextService _localizedTextService;

        public MembersNewLastWeekDashboardCounter(ILocalizedTextService localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public DashboardCounterModel GetModel(IScope scope)
        {
            var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember
                   inner join umbracoNode on cmsMember.nodeId = umbraconode.id     
                    WHERE createDate > @0", DateTime.Now.AddDays(-7));

            return new DashboardCounterModel()
            {
                Text = _localizedTextService.Localize("theDashboard/newMembersLastWeek"),
                Count = count,
                ClickUrl = "/umbraco#/member",
                Style = DashboardCounterModel.CounterStyles.Selected
            };
        }
    }
}
