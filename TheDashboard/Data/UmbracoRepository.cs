using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using TheDashboard.Data.DTO;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace TheDashboard.Data
{
    public class UmbracoRepository
    {
        #region Content dashboard

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
                ", DateTime.Now.AddDays(-7));
            return res;
        }

        public IEnumerable<UnpublishedNode> GetUnpublishedContent(int userId = 0)
        {
            //TODO: What about nodes thats intentional left unpublished?

            var userSqlFilter = string.Empty;
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


            return logItems.OrderByDescending(x => x.Timestamp).Where(x => x.NodeId != -1);
        }

        #endregion

        #region Developer dashboard

        public IEnumerable<ControllerDetailDto> GetControllersAssignableFrom(Type baseType, bool populateOnlyGetActionMethods = false)
        {
            return GetNonCoreTypesAssignableFrom(baseType)
                .Select(x => new ControllerDetailDto
                {
                    Name = x.Name,
                    Namespace = x.Namespace,
                    ActionMethods = GetActionMethodsOnController(x, populateOnlyGetActionMethods),
                });
        }

        public IEnumerable<ReflectedClassDto> GetApplicationEventHandlers()
        {
            return GetNonCoreTypesAssignableFrom(typeof(IApplicationEventHandler))
                .Select(x => new ReflectedClassDto
                {
                    Name = x.Name,
                    Namespace = x.Namespace,
                });
        }

        public IEnumerable<CustomEventDto> GetCustomEvents(IService serviceInstance)
        {
            return serviceInstance.GetType().GetEvents()
                .Select(x => new CustomEventDto
                {
                    EventName = x.Name,
                    Handlers = GetCustomEventHandlers(serviceInstance, x)
                })
                .Where(x => x.Handlers != null && x.Handlers.Any());
        }

        public IEnumerable<ReflectedClassDto> GetContentFinders()
        {
            return GetNonCoreTypesAssignableFrom(typeof(IContentFinder))
                .Select(x => new ReflectedClassDto
                {
                    Name = x.Name,
                    Namespace = x.Namespace,
                });
        }

        private IEnumerable<Type> GetNonCoreTypesAssignableFrom(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && !p.Namespace.ToLower().StartsWith("umbraco."));
        }

        private static IEnumerable<string> GetActionMethodsOnController(Type controllerType, bool onlyGetMethods)
        {
            return controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => typeof(ActionResult).IsAssignableFrom(x.ReturnType) &&
                            !x.Name.StartsWith("get_") &&
                            (!onlyGetMethods || !x.GetCustomAttributes(typeof(HttpPostAttribute), false).Any()))
                .Select(x => x.Name)
                .OrderBy(x => x);
        }

        private static IEnumerable<ReflectedClassDto> GetCustomEventHandlers(IService serviceInstance, EventInfo eventInfo)
        {
            var fi = GetEventField(serviceInstance, eventInfo);
            if (fi != null)
            {
                var del = fi.GetValue(serviceInstance) as Delegate;
                if (del != null)
                {
                    return del.GetInvocationList()
                        .Where(x => !x.Method.DeclaringType.FullName.ToLower().StartsWith("umbraco."))
                        .Select(x => new ReflectedClassDto
                        {
                            Name = x.Method.Name,
                            Namespace = x.Method.DeclaringType.FullName,
                        });
                }
            }

            return null;
        }

        private static FieldInfo GetEventField(IService serviceInstance, EventInfo eventInfo)
        {
            return serviceInstance.GetType()
                .GetField(eventInfo.Name,
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.FlattenHierarchy);
        }

        #endregion

        #region User dashboard


        public IEnumerable<UserActivityDto> GetUserActivities(DateTime date)
        {
            try
            {
                //if this date then all
                if (date.ToString("yyyy-MM-dd") == "1987-05-09")
                {
                    var useractivities = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<UserActivityDto>(@"
                    SELECT NodeId, Datestamp, logHeader, logComment, userName, text, uUserType.userTypeAlias
                        FROM umbracoLog uLog
                            LEFT JOIN umbracoUser uUser ON uLog.userId = uUser.id
                            LEFT JOIN umbracoNode uNode ON uLog.NodeId = uNode.id
    	    				INNER JOIN umbracoUserType uUserType ON uUser.userType = uUserType.id
                            ORDER BY Datestamp DESC");
                    return useractivities;
                    
                }
                else {
                    var useractivities = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<UserActivityDto>(@"
                    SELECT NodeId, Datestamp, logHeader, logComment, userName, text, uUserType.userTypeAlias
                        FROM umbracoLog uLog
                            LEFT JOIN umbracoUser uUser ON uLog.userId = uUser.id
                            LEFT JOIN umbracoNode uNode ON uLog.NodeId = uNode.id
					        INNER JOIN umbracoUserType uUserType ON uUser.userType = uUserType.id
                            WHERE CAST(Datestamp AS DATE) = '" + date.ToString("yyyy-MM-dd") + "' " +
                            "ORDER BY Datestamp DESC");
                    return useractivities;
                }
            }
            catch (Exception ex)
            {
                //Otherwise, you can use this code, that gets type automatically
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "thedashboardusermsg", ex);
                throw ex;

            }
        }

        public IEnumerable<DateTime> GetAvailableDates()
        {
            try
            {
                var useractivityDates = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<DateTime>(@"
                SELECT CAST(Datestamp AS DATE) AS DateTime
	                FROM umbracoLog uLog GROUP BY CAST(Datestamp AS DATE) ORDER BY DateTime DESC");
                return useractivityDates;

            }
            catch (Exception ex)
            {

                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "thedashboardusermsg", ex);
                throw ex;

            }
        }
        /**
        public IEnumerable<UserActivityDto> GetUserActivitiesSearch(string searchTerm)
        {
            try
            {
                if (!String.IsNullOrEmpty(searchTerm))
                {
                    string upperTerm = searchTerm.Replace("text: ", "").Trim().ToUpper();
                    var useractivities = UmbracoContext.Current.Application.DatabaseContext.Database.Fetch<UserActivityDto>(@"
                SELECT NodeId, Datestamp, logHeader, logComment, userName, text, uUserType.userTypeAlias
                    FROM umbracoLog uLog
                    LEFT JOIN umbracoUser uUser ON uLog.userId = uUser.id
                    LEFT JOIN umbracoNode uNode ON uLog.NodeId = uNode.id
					INNER JOIN umbracoUserType uUserType ON uUser.userType = uUserType.id
                    WHERE logHeader <> 'Open' AND (UPPER(userName) LIKE '%"+upperTerm+"%' OR UPPER(text) LIKE '%"+upperTerm+"%' OR UPPER(logComment) LIKE '%"+upperTerm+"%')" +
                    " ORDER BY Datestamp DESC, userName, text, logComment");

                    LogHelper.Info(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Search term is get user act:" + upperTerm);

                    //WHERE logHeader <> 'Open' AND (UPPER(userName) LIKE '%"+upperTerm+"%' OR UPPER(text) LIKE '%"+upperTerm+"%' OR UPPER(logComment) LIKE '%"+upperTerm+"%')" +

                    return useractivities;
                }
                else
                {
                    return new List<UserActivityDto>();
                }
            }
            catch (Exception ex)
            {
                //Otherwise, you can use this code, that gets type automatically

                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "thedashboardusermsg", ex);

                throw ex;

            }
        }**/


        #endregion
    }
}