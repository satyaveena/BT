CREATE PROCEDURE [dbo].[admdta_LoadTrackingProperties]
@strAppInstance nvarchar(50)
AS
	SELECT	Grp.TrackingDBServerName,
			Grp.TrackingDBName
	FROM 	adm_HostInstance AS HostInst,
			adm_Server2HostMapping AS SAM,
			adm_Host AS AppType,
			adm_Group AS Grp	
	WHERE	HostInst.UniqueId = @strAppInstance AND
			HostInst.Svr2HostMappingId = SAM.Id AND
			SAM.HostId = AppType.Id AND
			AppType.GroupId = Grp.Id

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_LoadTrackingProperties] TO [BTS_HOST_USERS]
    AS [dbo];

