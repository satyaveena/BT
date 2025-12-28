CREATE PROCEDURE [dbo].[btsmon_DTAOrphanedInstances]
@nIssues bigint output
AS
 --Check for Orphaned instances in DTA 
 declare @Server sysname
 declare @Name sysname
 declare @tsql nvarchar(4000)
 declare @i int
 
 SELECT @Server = TrackingDBServerName, @Name = TrackingDBName FROM [dbo].[adm_Group] 

 set @tsql = 'SELECT @nIssues  = COUNT(*) FROM [' + @Server + '].[' + @Name +'].[dbo].[btsmon_RunningInstances] WHERE
  [uidServiceInstanceId] NOT IN (
 '
 
 declare MsgboxDB_Cursor insensitive cursor for
 SELECT DBServerName, DBName FROM [dbo].[adm_MessageBox]
 
 open MsgboxDB_Cursor
 fetch next from MsgboxDB_Cursor into @Server, @Name
 set @i = 0
 while @@fetch_status = 0
 begin
  if (@i != 0)
  begin
   set @tsql = @tsql +  N' UNION ALL '
  end
  set @i = 1
  set @tsql = @tsql +  N'
   SELECT * FROM [' + @Server + '].[' + @Name +'].[dbo].[btsmon_RunningInstances]
  '
  
  fetch next from MsgboxDB_Cursor into @Server, @Name
 end
 close MsgboxDB_Cursor 
 deallocate MsgboxDB_Cursor
 
 set @tsql = @tsql + N')
 '
 set @nIssues = 0
 exec sp_executesql @tsql, N'@nIssues bigint output', @nIssues = @nIssues output
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsmon_DTAOrphanedInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];

