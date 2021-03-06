/*
Get latest activites for all users
*/
SELECT ul.[id]
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
	LEFT OUTER JOIN umbracoContentSchedule as ucs ON ucs.nodeId = ul.NodeId

  WHERE 
	ul.userId is not NULL 
	AND ul.nodeId is not NULL
	AND ul.nodeId > 0 -- Only include actual nodes
	AND ul.entityType = 'Document' 

  ORDER by ul.Datestamp DESC


