CREATE PROCEDURE [dbo].[sp_GetBackupDatabasesForServer]
@ServerName sysname = null
AS
 set transaction isolation level read committed
 set deadlock_priority low
 set nocount on

 SELECT ServerName, DatabaseName FROM admv_BackupDatabases
 WHERE @ServerName IS NULL OR (ServerName = @ServerName)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetBackupDatabasesForServer] TO [BTS_BACKUP_USERS]
    AS [dbo];

