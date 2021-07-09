using System;
using System.Threading;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Core;

namespace Our.Umbraco.TheDashboard.Counters.Implement
{
    public class ContentTotalContentItemsDashboardCounter : IDashboardCounter
    {
        private readonly ILocalizedTextService _localizedTextService;

        public ContentTotalContentItemsDashboardCounter(ILocalizedTextService localizedTextService)
        {
            _localizedTextService = localizedTextService;
        }

        public DashboardCounterModel GetModel(IScope scope)
        {
            var sql = @"SELECT count(un.[id])
                      FROM umbracoNode AS un
	                    INNER JOIN umbracoDocument as ud on ud.nodeId = un.id
                      WHERE 
	                    un.nodeObjectType = @0
                        AND un.trashed = 0
	                    AND ud.published = 1";

            var count = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document.ToString());

            return new DashboardCounterModel()
            {
                Text = _localizedTextService.Localize("theDashboard","publishedContentNodes", Thread.CurrentThread.CurrentCulture),
                Count = count,
                Style = DashboardCounterModel.CounterStyles.Action
            };
        }
    }
}
