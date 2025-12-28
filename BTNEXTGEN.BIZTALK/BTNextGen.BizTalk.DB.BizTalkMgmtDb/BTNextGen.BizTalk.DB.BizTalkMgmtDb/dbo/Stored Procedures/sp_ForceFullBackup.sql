CREATE PROCEDURE [dbo].[sp_ForceFullBackup]
AS
 set nocount on
 UPDATE [dbo].[adm_BackupSettings] SET [ForceFull] = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_ForceFullBackup] TO [BTS_BACKUP_USERS]
    AS [dbo];

