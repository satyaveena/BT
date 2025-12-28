create procedure [dbo].[admsvr_ReceivePortToPEP]
@service_port uniqueidentifier
AS
set nocount on
SELECT 		
		ad.Name,					-- InboundTransportType
		rl.InboundTransportURL,	-- InboundTransportLocation
		rl.InboundAddressableURL,-- InboundAddressableURL
		pl.PipelineID,			-- PipelineID
		ad.Capabilities			-- Capabilities, i.e. Receive, Send, SOAP, etc.
	FROM bts_orchestration_port op
	INNER JOIN bts_orchestration_port_binding opb ON  opb.nOrcPortID = op.nID	
	INNER JOIN adm_ReceiveLocation rl ON rl.ReceivePortId = opb.nReceivePortID
	INNER JOIN bts_pipeline pl ON pl.Id = rl.ReceivePipelineId
	LEFT JOIN adm_Adapter ad ON ad.Id = rl.AdapterId
	
		 
	WHERE	@service_port = op.uidGUID
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_ReceivePortToPEP] TO [BTS_HOST_USERS]
    AS [dbo];

