CREATE procedure [dbo].[admsvr_GetAppInstID]
@InboundURL nvarchar(1024),
@ServerName nvarchar(63),
@AppInstanceID uniqueidentifier OUTPUT,
@AppName nvarchar(256) OUTPUT,
@GroupName nvarchar(256) OUTPUT
AS
set nocount on
SELECT 	
	@AppInstanceID = appInst.UniqueId,
	@AppName = appType.Name,
	@GroupName = grp.Name
	FROM 	
		adm_ReceiveLocation recLoc, 
		adm_ReceiveHandler recHan, 
		adm_Server2HostMapping srv2Typ, 
		adm_HostInstance appInst,
		adm_Group grp, 
		adm_Host appType,
		adm_Server server
	WHERE	
		recLoc.InboundTransportURL = @InboundURL 		AND
		recLoc.ReceiveHandlerId = recHan.Id				AND
		recHan.HostId = srv2Typ.HostId					AND
		appInst.Svr2HostMappingId = srv2Typ.Id			AND
		srv2Typ.HostId = appType.Id						AND
		appType.GroupId = grp.Id						AND
		server.Name = @ServerName						AND
		srv2Typ.ServerId = server.Id					AND
		srv2Typ.HostId = appType.Id					
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetAppInstID] TO [BTS_HOST_USERS]
    AS [dbo];

