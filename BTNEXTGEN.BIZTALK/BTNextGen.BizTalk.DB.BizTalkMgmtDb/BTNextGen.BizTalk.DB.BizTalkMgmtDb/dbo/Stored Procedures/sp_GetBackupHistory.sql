CREATE PROCEDURE [dbo].[sp_GetBackupHistory] @LastBackupSetId bigint = NULL
AS
 BEGIN
 SET NOCOUNT ON

 declare @ret int
 /*
  Reader/writer conflict:
  Reader - Don't want to read records from sets that aren't finished writing
  but can't use a transaction to block readers because the "writing" - backing up a db - 
  can't be rolled back so the write to this table isn't rolled back.  
  Implementation - Use a table to hold the state of the writer and don't read while writing

  Writer - Don't want to start writing records while reader is reading and introduce a partial set
  to their read.
 */

 exec @ret = sp_AcquireBackupWriterLock
 if (@ret < 0)
 BEGIN
  return @ret
 END

 IF @LastBackupSetId IS NULL
  SELECT @LastBackupSetId = -1

 SELECT  [BackupId]
   ,[BackupSetId]
   ,[MarkName]
   ,[DatabaseName]
   ,[ServerName]
   ,[BackupFileName]
   ,[BackupFileLocation]
   ,[BackupType]
   ,[BackupDateTime]
   ,[SetComplete]
 FROM  [dbo].[adm_BackupHistory]
 WHERE  [BackupSetId] > @LastBackupSetId
 ORDER BY  [BackupSetId]

 exec @ret = sp_ReleaseBackupWriterLock

 RETURN 0

 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetBackupHistory] TO [BTS_BACKUP_USERS]
    AS [dbo];

