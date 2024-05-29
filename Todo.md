# The Dashboard TODOs

# Umbraco v14

* [ ] Migrate to use Guids over int-ids
  * [x] Recent
  * [x] Pending
* [ ] Ensure counter texts are translated on the frontend.
  * [ ] Remove any language-things from the server.


# Umbraco v14 - Upcoming
* [ ] Link to recycle bin when possible.

## Over all


* **Show deletes?** A problem is that we don't know the name of a deleted node..? 
* **LogTypes** we used to show Publish,Save,Delete and Unpublish

### All Logtypes from Core

**RollBack**
A RollBack will always be followed by a "Publish"-event when the page is published.

**Copy**

**Save**

**Sort**

**Move**

**Delete**

**Publish**

**Unpublish**

### LogTypes for Old The Dashboard
**Save**
saved but did not publish.
V8: When the LogHeader is "Saved" but no scheduleAction is set.

**SaveAndScheduled**
saved and scheduled for publish at
V8: When the LogHeader is "Saved" and scheduleAction is 'Release'

**Publish**
V8: LogHeader = Publish

**RecyleBin**
V8: LogHeader= Delete + Trashed in comment

**UnPublish**
V8: LogHeader = Unpublish

**Rollback**
Rolledback but not published
V8: LogHeader = Rollback


### What we need to know

**Recent activities**
* Node Name
* Node Id
* User Id
* User Name
* DateStamp
* Type of action (Save, publish, unpublish etc)


**Unpublished Content**



## Documentation and findings

### Unpublished Content

Content/Document nodeObjectType = 'C66BA18E-EAF3-4CFF-8A22-41B16D66A972'
Media nodeObjectType = 'B796F64C-1F99-4FFB-B886-4BF4BC011A9C'


Table [umbracoDocument] keeps a property "edited", that is 1 when the current published node is not live.
Table [umbracoContentSchedule] keeps a record for each item to be published


```sql
SELECT un.[id]
      ,[uniqueId]
      ,[parentId]
      ,[level]
      ,[path]
      ,[sortOrder]
      ,[trashed]
      ,[nodeUser]
      ,[text]
      ,[nodeObjectType]
      ,[createDate]
	  ,ud.edited
	  ,ud.published
	  ,ucs.action
  FROM umbracoNode AS un
	INNER JOIN umbracoDocument as ud on ud.nodeId = un.id
	LEFT OUTER JOIN umbracoContentSchedule as ucs on ucs.nodeId = un.id
  WHERE 
	un.nodeObjectType = 'C66BA18E-EAF3-4CFF-8A22-41B16D66A972' AND
	un.trashed = 0 AND ud.edited = 1 AND ud.published = 1 AND ucs.[action] is null
```


### Db: umbracoLog
Keeps log entries for "things" that happens in the db.

```sql
SELECT * FROM [umbracoLog] WHERE entityType = 'Document' order by Datestamp desc
```

#### Columns
* **id** identifier for the log entry
* **userId** User who performed the action
* **NodeId** Effected node id (can be null but does never seemt to be)
* **entityType** Indicates the entity type ie "Media" or "Document".
* **Datestamp** Time stamp
* **logHeader** Indicates "type" of action. Known values: Copy, RollBack, Save, Sort, Move, Delete, Publish, Unpublish
* **logComment** some comments in english.
* **parameters** seems to always be null


