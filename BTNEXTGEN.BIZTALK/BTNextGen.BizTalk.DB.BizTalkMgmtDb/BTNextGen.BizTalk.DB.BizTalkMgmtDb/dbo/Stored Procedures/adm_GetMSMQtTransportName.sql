CREATE PRoCEDURE [dbo].[adm_GetMSMQtTransportName]
@nvcMSMQtTransport nvarchar(256) OUTPUT
AS
SELECT @nvcMSMQtTransport = Name FROM adm_Adapter WHERE OutboundEngineCLSID = N'{0EE2AEC3-F646-41A6-80A1-A1AF5ED4F02B}'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_GetMSMQtTransportName] TO [BTS_ADMIN_USERS]
    AS [dbo];

