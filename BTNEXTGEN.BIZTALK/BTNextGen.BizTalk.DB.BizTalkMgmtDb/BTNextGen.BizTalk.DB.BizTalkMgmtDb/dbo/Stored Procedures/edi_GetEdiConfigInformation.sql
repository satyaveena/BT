CREATE PROCEDURE [dbo].[edi_GetEdiConfigInformation]
AS

BEGIN
	
	SELECT TOP 1 EdiEnabled, AS2Enabled, ReportingEnabled, SsoApplicationName
	FROM [dbo].[edi_DbConfig]
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetEdiConfigInformation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetEdiConfigInformation] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetEdiConfigInformation] TO [BTS_OPERATORS]
    AS [dbo];

