using System;
using System.Collections.Generic;
using System.Linq;
using TheDashboard.Data.DTO;
using umbraco.BusinessLogic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace TheDashboard.Data
{
    public class UmbracoRepository
    {

        public IEnumerable<IContent> GetRecycleBinNodes()
        {
            return UmbracoContext.Current.Application.Services.ContentService.GetContentInRecycleBin();
        }

        public int CountPublishedNodes()
        {

            var res =
                UmbracoContext.Current.Application.DatabaseContext.Database.ExecuteScalar<int>(@"SELECT COUNT(nodeId)
                    FROM [cmsDocument]
                    where published=1 and newest = 1");
            return res;
        }

         public int CountContentInRecycleBin()
        {
            //var res = UmbracoContext.Current.Application.DatabaseContext.Database.ExecuteScalar<int>(@"SELECT COUNT(nodeId) FROM [cmsDocument] inner join umbracoNode on cmsDocument.nodeId = umbracoNode.Id where umbracoNode.trashed = 1");
            var res = UmbracoContext.Current.Application.DatabaseContext.Database.ExecuteScalar<int>(@"SELECT COUNT(id) FROM umbracoNode where trashed = 1");
            return res;
        }
        

        public int CountMembers()
        {
            var res = UmbracoContext.Current.Application.DatabaseContext.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember");
            return res;
        }

        public int CountNewMember()
        {
            var res = UmbracoContext.Current.Application.DatabaseContext.Database.ExecuteScalar<int>(@"select COUNT(nodeId) from cmsMember
                inner join umbracoNode on cmsMember.nodeId = umbraconode.id     
                WHERE createDate > @0
            ",DateTime.Now.AddDays(-7));
            return res;
        }

        //select * from cmsMember 

        public IEnumerable<UnpublishedNode> GetUnpublishedContent(int userId = 0)
        {
            //TODO: What about nodes thats intentional left unpublished?

            string userSqlFilter = "";
            if (userId != 0)
            {
                userSqlFilter = " and documentUser=" + userId;
            }

            var unpublishedContent = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<UnpublishedNode>(@"
              SELECT [nodeId] ,[text], [documentUser], [releaseDate], [updateDate]
              FROM [cmsDocument]
              WHERE newest =1 and published = 0" + userSqlFilter);
              //WHERE newest =1 and published = 0 and releaseDate is null" + userSqlFilter);

            return unpublishedContent;
        }

        public IEnumerable<LogItem> GetLatestLogItems()
        {
            var dtLogItemsSince = DateTime.Now.AddDays(-183);

            var logItems = new List<LogItem>();

            logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Publish, dtLogItemsSince));
            logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Save, dtLogItemsSince));
            logItems.AddRange(Log.Instance.GetLogItems(LogTypes.Delete, dtLogItemsSince));
            logItems.AddRange(Log.Instance.GetLogItems(LogTypes.UnPublish, dtLogItemsSince));

            return logItems.OrderByDescending(x=>x.Timestamp).Where(x=>x.NodeId != -1);
        }

    }
}