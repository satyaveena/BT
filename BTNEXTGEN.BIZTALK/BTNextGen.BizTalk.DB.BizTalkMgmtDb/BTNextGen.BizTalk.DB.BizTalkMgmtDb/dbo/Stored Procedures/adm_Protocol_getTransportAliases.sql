create procedure [dbo].[adm_Protocol_getTransportAliases]
AS
set nocount on
SELECT a.Name,
       ISNULL(b.AliasValue, ''),
       a.OutboundEngineCLSID
	
FROM adm_Adapter a 
	LEFT OUTER JOIN adm_AdapterAlias b ON a.Id = b.AdapterId
WHERE a.OutboundEngineCLSID IS NOT NULL
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Protocol_getTransportAliases] TO [BTS_HOST_USERS]
    AS [dbo];

