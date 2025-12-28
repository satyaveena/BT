CREATE PROCEDURE [dbo].[sp_MarkAll] @MarkName nvarchar(8), @BackupPath nvarchar(128),@UseLocalTime bit = 0
AS
 begin

 set nocount on
 set xact_abort on

 declare @localized_string_sp_MarkAll_Failed_Executing_GetNextBackupSetId nvarchar(128)
 set @localized_string_sp_MarkAll_Failed_Executing_GetNextBackupSetId = N'Failed running sp_GetNextBackupSetId'

 declare @localized_string_sp_MarkAll_Failed_Executing_Backup nvarchar(128)
 set @localized_string_sp_MarkAll_Failed_Executing_Backup = N'Failed backing up the databases'

 declare @localized_string_sp_MarkAll_Failed_Executing_GetFileName nvarchar(128)
 set @localized_string_sp_MarkAll_Failed_Executing_GetFileName = N'Failed running sp_GetFileNameFromFilePath'

 declare @localized_string_sp_MarkAll_Timeout_Exceeded nvarchar(256)
 set @localized_string_sp_MarkAll_Timeout_Exceeded = N'Warning: A Remote Query Timeout may have occurred'

 declare @localized_string_sp_MarkAll_Failed_Log_Backup nvarchar(128)
 set @localized_string_sp_MarkAll_Failed_Log_Backup = N'Failed running the log backup on %s'

 declare @localized_string_sp_MarkAll_Failed_MarkBTSLogs nvarchar(128)
 set @localized_string_sp_MarkAll_Failed_MarkBTSLogs = N'sp_MarkAll failed running sp_MarkBTSLogs'


 declare  @ret int ,@error int ,@errorDesc nvarchar(128)
  ,@DbCount int ,@BackupSetId bigint ,@FullMarkName nvarchar(32)
  ,@BackupServer sysname ,@BackupDB sysname, @TimeStamp datetime
  ,@tsql nvarchar(1024)

 select @TimeStamp = 
  case @UseLocalTime
   when 0 then getutcdate()
   else getdate()
  end
  

 /*
  Get the backup set id
  Don't need to roll this back if we fail - we'll just skip that value
 */
 exec @BackupSetId = [dbo].[sp_GetNextBackupSetId]

 if @BackupSetId = -1
  begin
  select @errorDesc = @localized_string_sp_MarkAll_Failed_Executing_GetNextBackupSetId
  goto FAILED
  end

 SET @DbCount = 0
 exec [dbo].[sp_BuildFullMarkName] @MarkName, @TimeStamp,@UseLocalTime, @FullMarkName OUTPUT

 exec @ret = sp_AcquireBackupWriterLock

 exec @ret = sp_MarkBTSLogs @FullMarkName

 IF @@ERROR <> 0 OR @ret IS NULL OR @ret <> 0
  BEGIN
  SET @errorDesc = @localized_string_sp_MarkAll_Failed_MarkBTSLogs
  GOTO FAILED
  END

 
 /*
  Block any log shipping destination readers while history records for the set are written
  This isn't done in a transaction so that errors can be "shown" so an alternative method of blocking
  readers is needed.
 */
 
 declare @UseCompression bit
 select top 1 @UseCompression = [UseCompression] from [dbo].[adm_BackupSettings]
 
 DECLARE BackupDB_Cursor insensitive cursor for
 SELECT ServerName, DatabaseName
 FROM admv_BackupDatabases
 ORDER BY ServerName

 open BackupDB_Cursor 
 
 fetch next from BackupDB_Cursor into @BackupServer, @BackupDB
 
 while @@fetch_status = 0
  begin
  declare @rowcount int, @start datetime, @end datetime, @timeout int, @backup_loc nvarchar(4000), @filename nvarchar(500), @filelocation nvarchar(1500)

  set @start= getdate()    
 
  set @tsql = '[' + @BackupServer + '].[' + @BackupDB + '].[dbo].[sp_BackupBizTalkLog]'
  --@bCompression is made optional, so that custom DBs need not be upgraded
  if ((@UseCompression = 1) AND ([dbo].[sp_IsSSODB](@BackupServer, @BackupDB) = 0))
   exec @ret = @tsql @seq=@FullMarkName, @path=@BackupPath, @BackupLocation=@backup_loc output, @bCompression=@UseCompression    
  else
   exec @ret = @tsql @seq=@FullMarkName, @path=@BackupPath, @BackupLocation=@backup_loc output
  
  select @error = @@ERROR, @end=getdate()        
  if @error<>0 or @ret<>0 or @ret IS NULL      
  begin      
   exec @timeout=[dbo].[sp_GetLinkedServerQTimeout] @BackupServer
  
   if @timeout<=datediff(ss,@start,@end ) and @timeout != -1  
    print @localized_string_sp_MarkAll_Timeout_Exceeded    
   
   select @errorDesc = replace( @localized_string_sp_MarkAll_Failed_Log_Backup, '%s', @BackupServer + N'.' + @BackupDB )
   goto FAILED
  END      
  
  exec @ret = [dbo].[sp_GetFileNameFromFilePath] @FilePath = @backup_loc, @Name = @filename OUTPUT, @Location = @filelocation OUTPUT
  
  select @error = @@ERROR      
  if @error <> 0 or @ret IS NULL or @ret <> 0    
  begin     
   select @errorDesc = @localized_string_sp_MarkAll_Failed_Executing_GetFileName    
   goto FAILED     
  end        
  
  insert adm_BackupHistory(     BackupSetId     ,MarkName     ,DatabaseName    ,ServerName   ,BackupFileName     ,BackupFileLocation     ,BackupType     ,BackupDateTime    ) 
   values (     @BackupSetId     ,@FullMarkName     ,@BackupDB     ,@BackupServer  ,@filename     ,@BackupPath     ,'lg'     ,@TimeStamp    )         
  
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
  SET @errorDesc = @localized_string_sp_MarkAll_Failed_Executing_Backup
  GOTO FAILED
 END 

 goto DONE 

FAILED: 

 set @ret = -1
 raiserror( @errorDesc, 16, -1 )
 
 goto DONE


DONE: 
 /*
  Always attempt to release the writer lock
  Doesn't matter if we have it or not
 */
 exec @ret = sp_ReleaseBackupWriterLock

 return @ret
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_MarkAll] TO [BTS_BACKUP_USERS]
    AS [dbo];

