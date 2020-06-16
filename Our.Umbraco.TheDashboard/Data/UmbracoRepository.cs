namespace Our.Umbraco.TheDashboard.Data
{
    //public class UmbracoRepository
    //{

    //    //TODO: Replace with IScopeProvider

    //    #region Content dashboard

    //    public IEnumerable<IContent> GetRecycleBinNodes()
    //    {
    //        return UmbracoContext.Current.Application.Services.ContentService.GetContentInRecycleBin();
    //    }

  

   

    //    public IEnumerable<UnpublishedNode> GetUnpublishedContent(int userId = 0)
    //    {
    //        //TODO: What about nodes thats intentional left unpublished?

    //        var userSqlFilter = string.Empty;
    //        if (userId != 0)
    //        {
    //            userSqlFilter = " and documentUser=" + userId;
    //        }

    //        var unpublishedContent = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<UnpublishedNode>(@"
    //            SELECT [nodeId] ,[text], [documentUser], [releaseDate], [updateDate]
    //            FROM [cmsDocument]
    //            WHERE newest =1 and published = 0" + userSqlFilter);
    //            //WHERE newest =1 and published = 0 and releaseDate is null" + userSqlFilter);

    //        return unpublishedContent;
    //    }

    //    public IEnumerable<LogItem> GetLatestLogItems()
    //    {
    //        var dtLogItemsSince = DateTime.Now.AddDays(-183);

    //        var logItems = new List<LogItem>();

    //        logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Publish, dtLogItemsSince));
    //        logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Save, dtLogItemsSince));
    //        logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Delete, dtLogItemsSince));
    //        logItems.AddRange(Log.Instance.GetLogItems(LogTypes.UnPublish, dtLogItemsSince));

    //        return FilterLogItem(logItems).OrderByDescending(x=>x.Timestamp).Where(x=>x.NodeId != -1);
    //    }

    //    private IEnumerable<LogItem> FilterLogItem(IEnumerable<LogItem> logItems)
    //    {
    //        logItems = logItems.Where(x => x.NodeId != 0 && !x.Comment.Equals("Save DictionaryItem performed by user"));
    //        return logItems;
    //    }

    //    #endregion

    //}
}