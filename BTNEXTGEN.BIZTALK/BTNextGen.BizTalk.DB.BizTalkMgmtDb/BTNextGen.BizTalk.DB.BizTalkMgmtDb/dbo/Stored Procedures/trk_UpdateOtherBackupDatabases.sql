CREATE PROCEDURE [dbo].[trk_UpdateOtherBackupDatabases] 
(
	@DatabaseName nvarchar(128),
	@ServerName nvarchar(80),
	@DefaultDatabaseName nvarchar(128)
)
AS
BEGIN
	UPDATE adm_OtherBackupDatabases
	SET DatabaseName = @DatabaseName,
	    ServerName = @ServerName,
	    BTSServerName = @ServerName
	WHERE DefaultDatabaseName = @DefaultDatabaseName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_UpdateOtherBackupDatabases] TO [BTS_ADMIN_USERS]
    AS [dbo];

