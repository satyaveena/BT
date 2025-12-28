CREATE PROCEDURE [dbo].[btsmon_SubscriptionsWithoutInstances]
@nIssues bigint output
AS
 declare @Server sysname
 declare @Name sysname
 declare @tsql nvarchar(4000)
 declare @i int
 
 --IsMasterMsgBox is set to -1 for Master Message Box
 SELECT top 1 @Server = DBServerName, @Name = DBName FROM [dbo].[adm_MessageBox] WHERE IsMasterMsgBox = -1

 set @tsql = 'SELECT @nIssues  = COUNT(*) FROM [' + @Server + '].[' + @Name +'].[dbo].[btsmon_InstanceSubscriptions] WHERE
  [uidInstanceID] NOT IN (
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
   SELECT * FROM [' + @Server + '].[' + @Name +'].[dbo].[btsmon_Instances]
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
    ON OBJECT::[dbo].[btsmon_SubscriptionsWithoutInstances] TO [BTS_ADMIN_USERS]
    AS [dbo];

