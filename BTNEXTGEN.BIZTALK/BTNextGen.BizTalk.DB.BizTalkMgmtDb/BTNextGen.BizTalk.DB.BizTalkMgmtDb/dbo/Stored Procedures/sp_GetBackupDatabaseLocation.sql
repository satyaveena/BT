CREATE PROCEDURE [dbo].[sp_GetBackupDatabaseLocation]
@DefaultDatabaseName nvarchar(128)
AS
 SELECT ServerName, DatabaseName FROM adm_OtherBackupDatabases
 WHERE DefaultDatabaseName = @DefaultDatabaseName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetBackupDatabaseLocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetBackupDatabaseLocation] TO [BTS_OPERATORS]
    AS [dbo];

