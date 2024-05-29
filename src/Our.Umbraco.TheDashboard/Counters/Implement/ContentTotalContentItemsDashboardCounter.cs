using Umbraco.Cms.Core;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.TheDashboard.Counters.Implement;

public class ContentTotalContentItemsDashboardCounter : IDashboardCounter
{

    public ContentTotalContentItemsDashboardCounter()
    {
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

        var count = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document);

        return new DashboardCounterModel()
        {
            LocalizationKey = "theDashboard_publishedContentNodes",
            Count = count,
            Style = DashboardCounterModel.CounterStyles.Action
        };
    }
}
