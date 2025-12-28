create procedure [dbo].[adm_Protocol_getOutboundProtocols]
@AppName nvarchar(256)
AS
set nocount on
SELECT a.Name,
       a.DateModified,
       a.OutboundEngineCLSID,
       a.OutboundAssemblyPath,
       a.OutboundTypeName,
       a.PropertyNameSpace,
       b.uidCustomCfgID,
       a.Capabilities,
       b.CustomCfg AS DefaultTHCfg
	
FROM adm_Adapter a 
	INNER JOIN adm_SendHandler b ON a.Id = b.AdapterId
	INNER JOIN adm_Host host ON @AppName = host.Name
 WHERE 	b.HostId = host.Id
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Protocol_getOutboundProtocols] TO [BTS_HOST_USERS]
    AS [dbo];

