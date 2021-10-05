using System.Collections.Generic;
using System.Security.AccessControl;
using Our.Umbraco.TheDashboard.Models.Dtos;
using Umbraco.Core.Scoping;

namespace Our.Umbraco.TheDashboard.Services
{
    public class TheDashboardService : ITheDashboardService
    {
        private readonly IScopeProvider _scopeProvider;

        public TheDashboardService(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public List<LogEntryDto> GetEntries()
        {
            using (var scope = _scopeProvider.CreateScope())
            {
                //TODO: Add some kind of "limitation" on this one either count limit or date.
                //TODO: Only fetch logHeaders that we're interested in.

                var sql = @"SELECT TOP(500) ul.[id]
                              ,ul.[userId]
                              ,ul.[NodeId]
                              ,ul.[entityType]
                              ,ul.[Datestamp]
                              ,ul.[logHeader]
                              ,ul.[logComment]
	                          ,un.[text] as NodeName
                              ,un.[path] as NodePath
                              ,ucs.[date] as NodeScheduledDate
	                          ,uu.userName
	                          ,uu.userEmail
	                          ,uu.avatar as userAvatar
                          FROM umbracoLog as ul
	                        INNER JOIN umbracoNode as un ON un.id = ul.NodeId
	                        INNER JOIN umbracoUser as uu ON uu.id = ul.userId
                            LEFT OUTER JOIN umbracoContentSchedule as ucs ON ucs.nodeId = ul.NodeId
                          WHERE 
	                        ul.userId is not NULL 
	                        AND ul.nodeId is not NULL
	                        AND ul.nodeId > 0 -- Only include actual nodes
	                        AND ul.entityType = 'Document' 

                          ORDER by ul.Datestamp DESC";
                
                var res = scope.Database.Fetch<LogEntryDto>(sql);

                return res;
            }
        }

        public List<LogEntryDto> GetUnpublished()
        {

            var sql = @"SELECT ul.[id]
                  ,ul.[userId]
                  ,ul.[NodeId]
                  ,ul.[entityType]
                  ,ul.[Datestamp]
                  ,ul.[logHeader]
                  ,ul.[logComment]
	              ,un.[text] as NodeName
	              ,un.[path] as NodePath
	              ,ucs.[action] as NodeScheduleAction
	              ,uu.userName
	              ,uu.userEmail
	              ,uu.avatar as userAvatar
	              
              FROM umbracoLog as ul
	            INNER JOIN umbracoNode as un ON un.id = ul.NodeId
	            INNER JOIN umbracoUser as uu ON uu.id = ul.userId
	            INNER JOIN umbracoDocument as ud on ud.nodeId = un.id
	            LEFT OUTER JOIN umbracoContentSchedule as ucs ON ucs.nodeId = ul.NodeId

              WHERE 
	            ul.userId is not NULL 
	            AND ul.nodeId is not NULL
	            AND ul.nodeId > 0 -- Only include actual nodes
	            AND ul.entityType = 'Document' 
	            AND	un.trashed = 0 
	            AND ud.published = 0 
	            AND ucs.[action] is null
	            AND (ul.logHeader = 'Save' OR ul.logHeader = 'SaveVariant' or ul.logHeader = 'Unpublish')

              ORDER by ul.Datestamp DESC
            ";

            using (var scope = _scopeProvider.CreateScope())
            {

                var res = scope.Database.Fetch<LogEntryDto>(sql);
                return res;
            }

        }

        public List<LogEntryDto> GetPending()
        {

            var sql = @"SELECT ul.[id]
                  ,ul.[userId]
                  ,ul.[NodeId]
                  ,ul.[entityType]
                  ,ul.[Datestamp]
                  ,ul.[logHeader]
                  ,ul.[logComment]
	              ,un.[text] as NodeName
	              ,un.[path] as NodePath
	              ,ucs.[action] as NodeScheduleAction
	              ,uu.userName
	              ,uu.userEmail
	              ,uu.avatar as userAvatar
	              
              FROM umbracoLog as ul
	            INNER JOIN umbracoNode as un ON un.id = ul.NodeId
	            INNER JOIN umbracoUser as uu ON uu.id = ul.userId
	            INNER JOIN umbracoDocument as ud on ud.nodeId = un.id
	            LEFT OUTER JOIN umbracoContentSchedule as ucs ON ucs.nodeId = ul.NodeId

              WHERE 
	            ul.userId is not NULL 
	            AND ul.nodeId is not NULL
	            AND ul.nodeId > 0 -- Only include actual nodes
	            AND ul.entityType = 'Document' 
	            AND	un.trashed = 0 
	            AND ud.edited = 1 
	            AND ud.published = 1 
	            AND ucs.[action] is null
	            AND (ul.logHeader = 'Save' OR ul.logHeader = 'SaveVariant')

              ORDER by ul.Datestamp DESC
            ";

            using (var scope = _scopeProvider.CreateScope())
            {

                var res = scope.Database.Fetch<LogEntryDto>(sql);
                return res;
            }

        }
    }
}

