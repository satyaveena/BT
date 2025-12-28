CREATE PROCEDURE [dbo].[sp_GetServerVersion]
@version int output
AS
declare @Build nvarchar(20)
set @Build = CONVERT(nvarchar(20), SERVERPROPERTY('productversion'))
set @version = CONVERT(int, LEFT(@Build, CHARINDEX(N'.', @Build, 0) - 1))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sp_GetServerVersion] TO [BTS_BACKUP_USERS]
    AS [dbo];

