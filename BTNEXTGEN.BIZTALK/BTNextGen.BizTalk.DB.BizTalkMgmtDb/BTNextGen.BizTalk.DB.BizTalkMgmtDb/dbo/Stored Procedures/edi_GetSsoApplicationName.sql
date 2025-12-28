CREATE PROCEDURE [dbo].[edi_GetSsoApplicationName]
AS

BEGIN
	
	SELECT TOP 1 SsoApplicationName
	FROM [dbo].[edi_DbConfig]
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetSsoApplicationName] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetSsoApplicationName] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetSsoApplicationName] TO [BTS_OPERATORS]
    AS [dbo];

