CREATE PROCEDURE [dbo].[sp_SetBackupCompression] 
@bCompression bit
AS
 Update [dbo].[adm_BackupSettings] set [UseCompression] = @bCompression
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_SetBackupCompression] TO [BTS_BACKUP_USERS]
    AS [dbo];

