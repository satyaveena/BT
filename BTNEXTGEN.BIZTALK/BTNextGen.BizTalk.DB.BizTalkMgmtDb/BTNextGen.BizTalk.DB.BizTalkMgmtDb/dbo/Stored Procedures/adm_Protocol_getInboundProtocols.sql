create procedure [dbo].[adm_Protocol_getInboundProtocols]
@AppName nvarchar(256)
AS
set nocount on
SELECT a.Name,
       a.DateModified,
       a.InboundEngineCLSID,
       a.InboundAssemblyPath,
       a.InboundTypeName,
       a.PropertyNameSpace,
       b.uidCustomCfgID,
       a.Capabilities,
       b.CustomCfg AS DefaultRHCfg
  FROM 	adm_Adapter a 
  	INNER JOIN adm_ReceiveHandler b ON a.Id = b.AdapterId
	INNER JOIN adm_Host host ON @AppName = host.Name
 WHERE 	b.HostId = host.Id
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Protocol_getInboundProtocols] TO [BTS_HOST_USERS]
    AS [dbo];

