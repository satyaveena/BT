create procedure [dbo].[admsvr_GetSendPortAddress]
@service_port uniqueidentifier
AS
set nocount on
SELECT 	f.Name,					-- InboundTransportType
		b.nvcAddress,			-- InboundTransportLocation
		e.PipelineID			-- PipelineID
	FROM bts_sendport sp, bts_sendport_transport b, bts_orchestration_port c, 
		 bts_orchestration_port_binding d, bts_pipeline e, adm_Adapter f
		 
	WHERE	@service_port = c.uidGUID			AND
			c.nID = d.nOrcPortID				AND
			d.nSendPortID = sp.nID				AND
			sp.nSendPipelineID = e.Id			AND
			b.nSendPortID = sp.nID				AND
			b.nTransportTypeId = f.Id
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetSendPortAddress] TO [BTS_HOST_USERS]
    AS [dbo];

