create procedure [dbo].[admsvr_GetSendPort]
@SendPortId uniqueidentifier
AS
set nocount on
declare @uidTransformGUID uniqueidentifier
SELECT @uidTransformGUID = sp_trans.uidTransformGUID
	FROM bts_sendport sp
	LEFT JOIN bts_sendport_transform sp_trans ON sp_trans.nSendPortID = sp.nID
	WHERE	sp.uidGUID = @SendPortId 
SELECT 	sp.nID,
		sp.nvcName,
		sp.nvcEncryptionCertHash,
		sp.nvcSendPipelineData,			-- Send pipe line custom config
		sp.nvcReceivePipelineData,		-- Receive pipe line customer config (solicit-response case)
		spt.nvcAddress,
		spt.nRetryCount,
		spt.nRetryInterval,
		spt.bIsServiceWindow,
		spt.dtFromTime,
		spt.dtToTime,
		spt.nTransportTypeId,
		spt.nvcTransportTypeData,
		spt.bIsPrimary,
		spt.bSSOMappingExists,
		pl.PipelineID,
		sp.bDynamic,
		sp.uidGUID,
		sp.DateModified,
		pro.Name,							-- Protcol Name, e.g. FILE
		pro.OutboundEngineCLSID,
		pro.OutboundAssemblyPath,
		pro.OutboundTypeName,
		pro.PropertyNameSpace,
		rcv_pl.PipelineID,					-- Receive pipeline for solicit-response
		sp.nTracking,
		sp.bTwoWay,
		spt.uidGUID,						-- Send Port Transport GUID
		@uidTransformGUID,
		sh.uidTransmitLocationSSOAppId,		-- Send Handler SSO Application Id
		spt.bOrderedDelivery, 
		sp.bStopSendingOnFailure, 
		sp.bRouteFailedMessage,
		CASE WHEN (bam.uidPortId) IS NULL THEN CAST(0 as int) ELSE CAST(1 as int) END AS IsBamEnabled
		
	FROM bts_sendport sp
	LEFT JOIN bts_sendport_transport spt ON spt.nSendPortID = sp.nID
	LEFT JOIN adm_Adapter pro ON pro.Id = spt.nTransportTypeId
	INNER JOIN bts_pipeline pl ON pl.Id = sp.nSendPipelineID
	LEFT JOIN bts_pipeline rcv_pl ON rcv_pl.Id = sp.nReceivePipelineID
	LEFT JOIN adm_SendHandler sh ON sh.Id = spt.nSendHandlerID
	LEFT JOIN (SELECT DISTINCT(uidPortId) FROM bam_TrackPoints) bam ON sp.uidGUID = bam.uidPortId
	WHERE	sp.uidGUID = @SendPortId AND 
			((sp.bDynamic = 0 AND spt.nTransportTypeId IS NOT NULL) OR (sp.bDynamic = 1))
	
	ORDER BY sp.nID, spt.bIsPrimary DESC		
	
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetSendPort] TO [BTS_HOST_USERS]
    AS [dbo];

