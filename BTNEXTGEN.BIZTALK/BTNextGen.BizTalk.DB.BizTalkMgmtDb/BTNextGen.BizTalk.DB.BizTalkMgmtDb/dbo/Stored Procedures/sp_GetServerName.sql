CREATE PROCEDURE [dbo].[sp_GetServerName] @name sysname OUTPUT
AS
BEGIN
 set @name = CONVERT(sysname, SERVERPROPERTY('servername'))

 return 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetServerName] TO [BTS_BACKUP_USERS]
    AS [dbo];

