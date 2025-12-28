CREATE procedure [dbo].[admsvr_SendPortToPEP]
@service_port uniqueidentifier
AS
set nocount on
declare @nSendPortID int, @nSpgID as int
SELECT @nSendPortID = nSendPortID, @nSpgID = nSpgID
FROM bts_orchestration_port_binding spb
	INNER JOIN bts_orchestration_port srvp ON srvp.nID = spb.nOrcPortID
WHERE @service_port = srvp.uidGUID
IF (@@ROWCOUNT = 0) -- IF (@nSendPortID is null)
		return
IF (@nSendPortID = 0 AND @nSpgID = 0)
		return
	--	
	-- Distribution List Lookup
	--
	IF ( (@nSendPortID IS NULL) OR (@nSendPortID = 0) )
		BEGIN
			SELECT  NULL,
					NULL,
					NULL,
					uidGUID,					-- send port GUID
					NULL,						-- InboundTransportLocation
					NULL						-- Protcol Name, e.g. FILE
				FROM 	bts_sendportgroup
				WHERE nID = @nSpgID
		END
	ELSE IF ( (@nSpgID IS NULL) OR (@nSpgID = 0) )
		BEGIN
			SELECT  sp.uidGUID,
					spt.uidGUID,
					spt.bIsPrimary,
					@nSpgID,
					spt.nvcAddress,						-- InboundTransportLocation
					pro.Name							-- Protcol Name, e.g. FILE
				FROM bts_sendport sp
				LEFT JOIN bts_sendport_transport spt ON spt.nSendPortID = sp.nID
				INNER JOIN bts_pipeline pl ON pl.Id = sp.nSendPipelineID
				LEFT JOIN adm_Adapter pro ON pro.Id = spt.nTransportTypeId
				LEFT JOIN bts_pipeline rcv_pl ON rcv_pl.Id = sp.nReceivePipelineID
				WHERE sp.nID = @nSendPortID AND
					( sp.bDynamic = 1 OR spt.nTransportTypeId IS NOT NULL)
				ORDER BY sp.nID, spt.bIsPrimary DESC				
		END
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_SendPortToPEP] TO [BTS_HOST_USERS]
    AS [dbo];

