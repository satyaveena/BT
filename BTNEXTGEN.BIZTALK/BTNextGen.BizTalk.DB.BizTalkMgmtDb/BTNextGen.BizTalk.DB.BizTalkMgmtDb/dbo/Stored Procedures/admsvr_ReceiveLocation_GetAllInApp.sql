CREATE procedure [dbo].[admsvr_ReceiveLocation_GetAllInApp]
@AppInstanceID uniqueidentifier
AS
set nocount on
SELECT 	rl.Name,
		dbo.admsvr_GetLastestDate(rl.DateModified,d.DateModified),
		rl.ActiveStartDT,
		rl.ActiveStopDT,
		rl.StartDTEnabled,
		dbo.adm_fnConvertLocalToUTCDate(rl.SrvWinStartDT),
		rl.StopDTEnabled,
		dbo.adm_fnConvertLocalToUTCDate(rl.SrvWinStopDT),
		rl.Disabled,
		rl.uidCustomCfgID,
		rl.bSSOMappingExists,
		rl.InboundTransportURL,
		rl.OperatingWindowEnabled,
		rl.ReceivePipelineData,		-- Custom Pipeline configuration data
		h.PipelineID,
		d.uidGUID,					-- Receive Port ID
		d.nvcName,                  -- Receive Port Name
		e.Name,		-- Protcol Name, e.g. FILE
		e.InboundEngineCLSID,
		i.PipelineID,
		d.bTwoWay,
		d.nAuthentication,
		rl.SendPipelineData,		-- Custom Pipeline config (req-resp)
		d.nTracking,
		b.uidReceiveLocationSSOAppID,
		rl.CustomCfg,
		dbo.admsvr_CheckForMapOnReceivePort(d.nID),
		j.PipelineID,
		rl.EncryptionCertThumbPrint,
		d.bRouteFailedMessage,
		CASE WHEN (bam.uidPortId) IS NULL THEN CAST(0 as int) ELSE CAST(1 as int) END AS IsBamEnabled
	FROM 	
		adm_ReceiveLocation rl
		INNER JOIN adm_ReceiveHandler b ON b.Id = rl.ReceiveHandlerId
		INNER JOIN adm_Adapter e ON b.AdapterId = e.Id
		INNER JOIN adm_Server2HostMapping f ON b.HostId = f.HostId
		INNER JOIN adm_HostInstance g ON g.Svr2HostMappingId = f.Id   
		INNER JOIN bts_pipeline h ON h.Id = rl.ReceivePipelineId
		INNER JOIN  bts_receiveport d ON d.nID = rl.ReceivePortId
		LEFT JOIN bts_pipeline i ON i.Id = d.nSendPipelineId
		LEFT JOIN bts_pipeline j on j.Id = rl.SendPipelineId
		LEFT JOIN (SELECT DISTINCT(uidPortId) FROM bam_TrackPoints) bam ON d.uidGUID = bam.uidPortId
	WHERE 	
		g.UniqueId = @AppInstanceID
		
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_ReceiveLocation_GetAllInApp] TO [BTS_HOST_USERS]
    AS [dbo];

