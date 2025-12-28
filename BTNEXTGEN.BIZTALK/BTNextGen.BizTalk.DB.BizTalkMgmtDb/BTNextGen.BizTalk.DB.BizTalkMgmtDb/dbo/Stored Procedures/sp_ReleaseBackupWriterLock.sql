CREATE PROCEDURE [dbo].[sp_ReleaseBackupWriterLock]
AS
 BEGIN
 set nocount on
 declare @ret int

 exec @ret = sp_releaseapplock 'BTS_BackupJob_Lock', 'Session'
 return @ret
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_ReleaseBackupWriterLock] TO [BTS_BACKUP_USERS]
    AS [dbo];

