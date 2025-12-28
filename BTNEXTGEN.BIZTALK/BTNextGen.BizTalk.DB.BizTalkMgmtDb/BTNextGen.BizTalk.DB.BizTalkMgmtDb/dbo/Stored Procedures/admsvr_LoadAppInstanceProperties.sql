CREATE PROCEDURE [dbo].[admsvr_LoadAppInstanceProperties]
@uidAppInstance uniqueidentifier,
@nvcAppName	nvarchar(256) OUTPUT,
@nvcMasterDBServer nvarchar(80) OUTPUT,
@nvcMasterDBName nvarchar(128) OUTPUT,
@nNumMsgboxServers int OUTPUT,
@nCacheRefreshInterval int OUTPUT
AS
declare @id int
set @nNumMsgboxServers = 0
SELECT @nvcAppName = AppType.Name,
	@nvcMasterDBServer = Grp.SubscriptionDBServerName,
	@nvcMasterDBName = Grp.SubscriptionDBName,
	@id = Grp.Id,
	@nCacheRefreshInterval = Grp.ConfigurationCacheRefreshInterval
FROM 	adm_HostInstance AS AppInst,
	adm_Server2HostMapping AS SAM,
	adm_Host AS AppType,
	adm_Group AS Grp	
WHERE AppInst.UniqueId = @uidAppInstance AND
	AppInst.Svr2HostMappingId = SAM.Id AND
	SAM.HostId = AppType.Id AND
	AppType.GroupId = Grp.Id
SELECT MsgBx.DBServerName, MsgBx.DBName, MsgBx.UniqueId, MsgBx.DisableNewMsgPublication, MsgBx.IsMasterMsgBox
FROM	 adm_MessageBox AS MsgBx
WHERE MsgBx.GroupId = @id
set @nNumMsgboxServers = @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_LoadAppInstanceProperties] TO [BTS_HOST_USERS]
    AS [dbo];

