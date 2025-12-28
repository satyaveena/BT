create procedure [dbo].[sp_BackupAllFull] @MarkName nvarchar(8), @BackupPath nvarchar(128), @TimeStamp datetime = NULL,@UseLocalTime bit=0

as
 begin
 set nocount on
 set xact_abort on

 declare @localized_string_sp_BackupAllFull_Failed_Executing_GetNextBackupSetId nvarchar(128)
 set @localized_string_sp_BackupAllFull_Failed_Executing_GetNextBackupSetId = N'Failed running sp_GetNextBackupSetId'

 declare @localized_string_sp_BackupAllFull_Failed_Executing_Backup nvarchar(128)
 set @localized_string_sp_BackupAllFull_Failed_Executing_Backup = N'Failed backing up the databases'

 declare @localized_string_sp_BackupAllFull_Failed_Executing_GetFileName nvarchar(128)
 set @localized_string_sp_BackupAllFull_Failed_Executing_GetFileName = N'Failed running sp_GetFileNameFromFilePath'

 declare @localized_string_sp_BackupAllFull_Failed_Inserting_BackupHistory nvarchar(128)
 set @localized_string_sp_BackupAllFull_Failed_Inserting_BackupHistory = N'Failed inserting into adm_BackupHistory'

 declare @localized_string_sp_BackupAllFull_Failed_Backup nvarchar(128)
 set @localized_string_sp_BackupAllFull_Failed_Backup = N'Failed running the backup on %s'

 declare @localized_string_sp_BackupAllFull_Timeout_Exceeded nvarchar(256)
 set @localized_string_sp_BackupAllFull_Timeout_Exceeded = N'Warning: A Remote Query Timeout may have occurred'

 declare @localized_string_sp_BackupAllFull_sp_GetRemoteServerNameFailed nvarchar(128)
 set @localized_string_sp_BackupAllFull_sp_GetRemoteServerNameFailed = N'sp_GetRemoteServerName failed to resolve server name %s'

 declare  @ret int ,@error int ,@errorDesc nvarchar(256), @rowcount int
  ,@DbCount int ,@BackupSetId bigint ,@FullMarkName nvarchar(32)
  ,@BackupServer sysname ,@BackupDB sysname
  ,@RealServerName sysname ,@backup_loc nvarchar(4000) ,@filelocation nvarchar(4000) ,@filename nvarchar(500) 
  ,@tsql nvarchar(512)

 /*
  Get the backup set id
  Don't need to roll this back if we fail - we'll just skip that value
 */
 exec @BackupSetId = [dbo].[sp_GetNextBackupSetId]

 if @BackupSetId = -1
  begin
  select @errorDesc = @localized_string_sp_BackupAllFull_Failed_Executing_GetNextBackupSetId
  goto FAILED
  end

 SET @DbCount = 0
 exec [dbo].[sp_BuildFullMarkName] @MarkName, @TimeStamp, @UseLocalTime,@FullMarkName OUTPUT

 /*
  IMPORTANT NOTE!
  A transaction is not used around the set of inserts into adm_BackupHistory.
  This is because we must know if some backups succeeded and some failed (basically if 
  we have a complete set).  Because you can't rollback a backup action we don't want to 
  rollback that a backup action occurred.  If the set is complete (the number of rows for set
  in adm_BackupHistory matches the number of databases that we expect to back up then we update 
  adm_BackupHistory to indicate that the set is complete.

  The log shipping restore process depends on this functionality!  Do not change this non transactional
  logic without considering how it impacts the restore process under failure conditions.

  Block any log shipping destination readers while history records for the set are written
  This isn't done in a transaction so that errors can be "shown" so an alternative method of blocking
  readers is needed.
 */

 exec @ret = [dbo].[sp_AcquireBackupWriterLock]
 
 declare @UseCompression bit
 select top 1 @UseCompression = [UseCompression] from [dbo].[adm_BackupSettings]

 declare BackupDB_Cursor insensitive cursor for
 select ServerName, DatabaseName
 from admv_BackupDatabases
 
 open BackupDB_Cursor 
 
 fetch next from BackupDB_Cursor into @BackupServer, @BackupDB
 
 while @@fetch_status = 0
  begin
  
  --let's make sure we are linked to this server so that we can call the backup job on it
  exec bts_SafeAddLinkedServer @BackupServer

  set @tsql = '[' + @BackupServer + '].[' + @BackupDB + '].[dbo].[sp_BackupBizTalkFull]'  
  --@bCompression is made optional, so that custom DBs need not be upgraded
  if ((@UseCompression = 1) AND ([dbo].[sp_IsSSODB](@BackupServer, @BackupDB) = 0))
   exec @ret = @tsql @seq=@FullMarkName, @path=@BackupPath, @BackupLocation=@backup_loc output, @bCompression=@UseCompression
  else
   exec @ret = @tsql @seq=@FullMarkName, @path=@BackupPath, @BackupLocation=@backup_loc output
  
  select @error = @@ERROR
  if @error <> 0 or @ret <> 0 or @ret IS NULL
  begin       
   select @errorDesc = replace( @localized_string_sp_BackupAllFull_Failed_Backup, '%s', @BackupServer + N'.' + @BackupDB )
   goto FAILED     
  end      
  
  exec @ret = [dbo].[sp_GetFileNameFromFilePath] @FilePath = @backup_loc, @Name = @filename OUTPUT, @Location = @filelocation OUTPUT
  
  select @error = @@ERROR
  if @error <> 0 or @ret <> 0 or @ret is NULL
  begin     
   select @errorDesc = @localized_string_sp_BackupAllFull_Failed_Executing_GetFileName
   goto FAILED     
  end        
  
  insert adm_BackupHistory(     BackupSetId     ,DatabaseName    ,ServerName   ,BackupFileName     ,BackupFileLocation     ,BackupType     ,BackupDateTime    ) 
   values (    @BackupSetId    ,@BackupDB   ,@BackupServer   ,@filename     ,@BackupPath    ,'db'     ,@TimeStamp    )     

  SET @DbCount = @DbCount + 1

  fetch next from BackupDB_Cursor into @BackupServer, @BackupDB
  end
 
 close BackupDB_Cursor 
 deallocate BackupDB_Cursor 

 UPDATE [dbo].[adm_BackupHistory]
 SET [SetComplete] = 1
 WHERE [BackupSetId] = @BackupSetId

 SELECT @error  = @@ERROR
  ,@rowcount = @@ROWCOUNT

 IF @error <> 0 OR @rowcount <> @DbCount or @rowcount IS NULL
 BEGIN
  SET @errorDesc = @localized_string_sp_BackupAllFull_Failed_Executing_Backup
  GOTO FAILED
 END 

 goto DONE 

FAILED:

 raiserror( @errorDesc, 16, -1 )
 return -1

DONE:
 /*
  Always attempt to release the writer lock
  Doesn't matter if we have it or not
 */
 exec @ret = sp_ReleaseBackupWriterLock

 return 0

 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_BackupAllFull] TO [BTS_BACKUP_USERS]
    AS [dbo];

