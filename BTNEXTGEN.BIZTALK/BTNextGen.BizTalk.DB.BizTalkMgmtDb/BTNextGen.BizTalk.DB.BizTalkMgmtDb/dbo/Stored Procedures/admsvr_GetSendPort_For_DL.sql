create procedure [dbo].[admsvr_GetSendPort_For_DL]
@DLId int
AS
set nocount on
SELECT 	sp.nID,
		sp.nvcName,
		sp.nvcEncryptionCert,
		spt.nvcAddress,
		spt.nRetryCount,
		spt.nRetryInterval,
		spt.bIsServiceWindow,
		spt.dtFromTime,
		spt.dtToTime,
		spt.nTransportTypeId,
		spt.nvcTransportTypeData,
		spt.bIsPrimary,
		pl.PipelineID,
		sp.bDynamic,
		sp.uidGUID,
		sp.DateModified,
		pro.Name,							-- Protcol Name, e.g. FILE
		pro.InboundEngineCLSID,
		pro.InboundAssemblyPath,
		pro.InboundTypeName,
		pro.PropertyNameSpace,
		rcv_pl.PipelineID,					-- Receive pipeline for solicit-response
		sp.nTracking,
		sp.bTwoWay
		
	FROM bts_sendport sp
	INNER JOIN bts_spg_sendport e ON e.nSendPortID = sp.nID
	INNER JOIN bts_sendport_transport spt ON spt.nSendPortID = sp.nID
	INNER JOIN adm_Adapter pro ON pro.Id = spt.nTransportTypeId
	INNER JOIN bts_pipeline pl ON pl.Id = sp.nSendPipelineID
	LEFT JOIN bts_pipeline rcv_pl ON rcv_pl.Id = sp.nReceivePipelineID
	WHERE e.nSendPortGroupID = @DLId
	ORDER BY sp.nID, spt.bIsPrimary DESC					
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetSendPort_For_DL] TO [BTS_HOST_USERS]
    AS [dbo];

