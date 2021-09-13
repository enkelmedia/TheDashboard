using System.Threading;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace Our.Umbraco.TheDashboard.Counters.Implement
{
    public class ContentInRecycleBinDashboardCounter : IDashboardCounter
    {
        private readonly ILocalizedTextService _localizedTextService;

        public ContentInRecycleBinDashboardCounter(ILocalizedTextService localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public DashboardCounterModel GetModel(IScope scope)
        {
            var sql = @"SELECT count(un.[id]) FROM umbracoNode AS un
                               WHERE un.nodeObjectType = @0 	
	                           AND un.trashed = 1";

            var count = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document.ToString());

            return new DashboardCounterModel()
            {
                Text = _localizedTextService.Localize("theDashboard","nodesInRecycleBin",Thread.CurrentThread.CurrentCulture),
                Count = count,
                ClickUrl = "/umbraco#/content/content/recyclebin",
                Style = DashboardCounterModel.CounterStyles.Action
            };
        }
    }
}
