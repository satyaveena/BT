CREATE PROCEDURE [dbo].[sp_AcquireBackupWriterLock]
AS
 BEGIN
 set nocount on
 declare @ret int

 exec @ret = sp_getapplock  'BTS_BackupJob_Lock', 'Exclusive', 'Session'

 return @ret
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_AcquireBackupWriterLock] TO [BTS_BACKUP_USERS]
    AS [dbo];

