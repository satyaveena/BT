CREATE PROCEDURE [dbo].[sp_MarkBTSLogs]  @MarkName nvarchar(32)
AS
 BEGIN

 SET NOCOUNT ON

 DECLARE @BackupServer sysname, @BackupDB sysname, @RealServerName sysname, @LastServer sysname
  ,@ret int ,@error int ,@errorDesc nvarchar(128), @tsql nvarchar(1024)


 declare @localized_string_sp_MarkBTSLogs_Failed_sp_GetRemoteServerNameFailed nvarchar(128)
 set @localized_string_sp_MarkBTSLogs_Failed_sp_GetRemoteServerNameFailed = N'sp_GetRemoteServerName failed to resolve server name %s'

 declare @localized_string_sp_MarkBTSLogs_Failed_Setting_Mark nvarchar(128)
 set @localized_string_sp_MarkBTSLogs_Failed_Setting_Mark = N'Failed running the marking proc on %s'


 /*
  IMPORTANT!!
  The with mark syntax can only be executed once per server (not database) for 
  a given transaction without generating a warning.  The warning flags @@error.  In order to
  avoid these warnings we do 2 things:
   1. Use 2 stored proc to do the markings - sp_SetMark and sp_SetMarkRemote 
      The Remote version uses the with mark syntax.
   2. Group by the ServerName so we don't get too many servers to connect to.
 */
 
 /* 
     In order to handle connecting to the same server using different naming conventions (like port name and such)
  we need to actually go to each server and resolve the name we have to the actual name of the server. This will prevent
  the issue desribed above
 */

 CREATE TABLE #BackupDBs_MarkBTSLogs (ServerName sysname, DatabaseName sysname, RealServerName sysname)

 DECLARE BackupDB_Cursor insensitive cursor for
 SELECT ServerName, DatabaseName
 FROM admv_BackupDatabases
 ORDER BY ServerName
 
 open BackupDB_Cursor

 fetch next from BackupDB_Cursor into @BackupServer, @BackupDB

 WHILE (@@FETCH_STATUS = 0)
 BEGIN
  /*
   Get the real name of the server 
   This resolves names like "BTSDbServerName,5000" (or a client server name alias)
   to "BTSDbServerName" (server name as returned by serverproperty('servername'))
  */
  EXEC @ret = sp_GetRemoteServerName @ServerName = @BackupServer, @DatabaseName = @BackupDB, @RemoteServerName = @RealServerName OUTPUT
 
  IF @@ERROR <> 0 OR @ret IS NULL OR @ret <> 0 OR @RealServerName IS NULL OR len(@RealServerName) <= 0
   BEGIN
   SET @errorDesc = replace( @localized_string_sp_MarkBTSLogs_Failed_sp_GetRemoteServerNameFailed, N'%s', @BackupServer )
   RAISERROR( @errorDesc, 16, -1 )
   GOTO FAILED
   END

  INSERT INTO #BackupDBs_MarkBTSLogs (ServerName, DatabaseName, RealServerName) VALUES (@BackupServer, @BackupDB, @RealServerName)
  fetch next from BackupDB_Cursor into @BackupServer, @BackupDB
 END

 close BackupDB_Cursor
 deallocate BackupDB_Cursor

 /* now cursor over the above list and actually mark the logs */

 DECLARE BackupDB_Cursor insensitive cursor for
 SELECT ServerName, DatabaseName, RealServerName
 FROM #BackupDBs_MarkBTSLogs
 ORDER BY RealServerName
 
 BEGIN TRANSACTION @MarkName WITH MARK @MarkName

 --First Block TDDS
 exec sp_BlockTDDS

 open BackupDB_Cursor 
 
 fetch next from BackupDB_Cursor into @BackupServer, @BackupDB, @RealServerName
 WHILE (@@FETCH_STATUS = 0)
 BEGIN

  if ( (@RealServerName = CAST(SERVERPROPERTY('servername') as sysname)) OR (@RealServerName = @LastServer) )
  BEGIN
   --we already marked this so don't use the with mark syntax
   set @tsql = '[' + @BackupServer + '].[' + @BackupDB + '].[dbo].[sp_SetMark]'   
  END
  ELSE
  BEGIN
   set @tsql = '[' + @BackupServer + '].[' + @BackupDB + '].[dbo].[sp_SetMarkRemote]'
  END
     
  exec @ret = @tsql @MarkName
  select @error = @@ERROR   
  if @error <> 0 or @ret IS NULL or @ret <> 0
  begin
        select @errorDesc = replace( @localized_string_sp_MarkBTSLogs_Failed_Setting_Mark, '%s', @BackupServer + N'.' + @BackupDB )
   goto FAILED     
  end   
 
  set @LastServer = @RealServerName
  fetch next from BackupDB_Cursor into @BackupServer, @BackupDB, @RealServerName

 END

 close BackupDB_Cursor
 deallocate BackupDB_Cursor

 COMMIT TRANSACTION

 goto DONE

FAILED: 

 set @ret = -1
 raiserror( @errorDesc, 16, -1 )
 
 goto DONE


DONE: 
 return @ret
END