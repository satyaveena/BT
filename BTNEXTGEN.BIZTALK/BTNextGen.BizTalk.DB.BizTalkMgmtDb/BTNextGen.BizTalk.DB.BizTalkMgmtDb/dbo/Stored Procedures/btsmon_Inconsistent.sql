CREATE PROCEDURE [dbo].[btsmon_Inconsistent]
AS
 SET DEADLOCK_PRIORITY LOW

 TRUNCATE TABLE [dbo].[btsmon_Inconsistancies]
 
 declare @Server sysname
 declare @Name sysname
 declare @IsMasterMsgBox int
 declare @tsql nvarchar(400)
 
 declare @MessageRefsWithoutSpool bigint
 declare @MessagesWithoutRefcount bigint
 declare @MessagesRefsWithoutInstances bigint
 declare @SubscriptionWithoutInstances bigint
 declare @InstanceStateWithoutInstances bigint
 declare @NegativeRefCount bigint
 declare @MessagesWithoutReferences bigint
 declare @OrphanedDTAServiceInstanceExceptions bigint
 declare @OrphanedDTAServiceInstances bigint
 declare @TDDSNotRunning bigint
 
 declare MsgboxDB_Cursor insensitive cursor for
 SELECT DBServerName, DBName, IsMasterMsgBox FROM [dbo].[adm_MessageBox]
 
 open MsgboxDB_Cursor
 fetch next from MsgboxDB_Cursor into @Server, @Name, @IsMasterMsgBox
 while @@fetch_status = 0
 BEGIN
  set @NegativeRefCount = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_MessagesWithNegativeRefCount]'
  exec @tsql @nIssues = @NegativeRefCount output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 2, @NegativeRefCount)
  
  set @MessagesWithoutRefcount = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_MessagesWithoutRefcount]'
  exec @tsql @nIssues = @MessagesWithoutRefcount output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 3, @MessagesWithoutRefcount)
  
  set @MessagesWithoutReferences = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_MessagesWithoutReferences]'
  exec @tsql @nIssues = @MessagesWithoutReferences output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 4, @MessagesWithoutReferences)
  
  set @MessageRefsWithoutSpool = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_MessageRefsWithoutSpool]'
  exec @tsql @nIssues = @MessageRefsWithoutSpool output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 5, @MessageRefsWithoutSpool)
  
  set @MessagesRefsWithoutInstances = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_MessagesRefsWithoutInstances]'
  exec @tsql @nIssues = @MessagesRefsWithoutInstances output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 6, @MessagesRefsWithoutInstances)
  
  set @InstanceStateWithoutInstances = 0
  set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_InstanceStateWithoutInstances]'
  exec @tsql @nIssues = @InstanceStateWithoutInstances output
  INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 7, @InstanceStateWithoutInstances)
  
  fetch next from MsgboxDB_Cursor into @Server, @Name, @IsMasterMsgBox
 end
 close MsgboxDB_Cursor 
 
 set @SubscriptionWithoutInstances = 0
 SELECT top 1 @Server = DBServerName, @Name = DBName FROM [dbo].[adm_MessageBox] WHERE IsMasterMsgBox = -1
 exec [dbo].[btsmon_SubscriptionsWithoutInstances] @nIssues = @SubscriptionWithoutInstances output
 INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 1, @SubscriptionWithoutInstances)
 
 --Check for inconsistency in DTA
 set @OrphanedDTAServiceInstanceExceptions = 0
 SELECT @Server = TrackingDBServerName, @Name = TrackingDBName FROM [dbo].[adm_Group] 
 set @tsql = '[' + @Server + '].[' + @Name + '].[dbo].[btsmon_OrphanedDTAServieInstanceExceptions]'  
 exec @tsql @nIssues = @OrphanedDTAServiceInstanceExceptions output
 INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name, 8, @OrphanedDTAServiceInstanceExceptions)
 
 set @OrphanedDTAServiceInstances = 0
 exec [dbo].[btsmon_DTAOrphanedInstances] @nIssues = @OrphanedDTAServiceInstances output
 INSERT INTO [dbo].[btsmon_Inconsistancies] VALUES (@Server, @Name , 9, @OrphanedDTAServiceInstances)
 
 set @TDDSNotRunning = 0
 exec [dbo].[btsmon_TDDSRunning] @nIssues = @TDDSNotRunning output
 INSERT INTO [dbo].[btsmon_Inconsistancies] values (CONVERT(sysname, SERVERPROPERTY('SERVERNAME')), DB_NAME(), 10, @TDDSNotRunning)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsmon_Inconsistent] TO [BTS_ADMIN_USERS]
    AS [dbo];

