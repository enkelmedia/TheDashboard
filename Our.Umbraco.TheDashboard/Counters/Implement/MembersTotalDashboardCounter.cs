using System.Threading;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.TheDashboard.Counters.Implement
{
    public class MembersTotalDashboardCounter : IDashboardCounter
    {
        private readonly ILocalizedTextService  _localizedTextService;

        public MembersTotalDashboardCounter(ILocalizedTextService  localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public DashboardCounterModel GetModel(IScope scope)
        {
            var count = scope.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember");
            
            return new DashboardCounterModel()
            {
                Text = _localizedTextService.Localize("theDashboard","membersOnWebsite",Thread.CurrentThread.CurrentCulture),
                Count = count,
                ClickUrl = "/umbraco#/member/member/list/all-members",
                Style = DashboardCounterModel.CounterStyles.Selected
            };
        }
    }
}
