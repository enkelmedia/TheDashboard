using Umbraco.Cms.Core;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Our.Umbraco.TheDashboard.Counters.Implement;

public class ContentInRecycleBinDashboardCounter : IDashboardCounter
{

    public ContentInRecycleBinDashboardCounter()
    {
    }

    public DashboardCounterModel GetModel(IScope scope)
    {
        var sql = @"SELECT count(un.[id]) FROM umbracoNode AS un
                               WHERE un.nodeObjectType = @0 	
	                           AND un.trashed = 1";

        var count = scope.Database.ExecuteScalar<int>(sql, Constants.ObjectTypes.Document);
            
        return new DashboardCounterModel()
        {
            LocalizationKey = "theDashboard_nodesInRecycleBin",
            Count = count,
            //ClickUrl = "/umbraco#/content/content/recyclebin", //TODO: Add link in the future if core adds overview for recyle bin
            Style = DashboardCounterModel.CounterStyles.Action
        };
    }
}
