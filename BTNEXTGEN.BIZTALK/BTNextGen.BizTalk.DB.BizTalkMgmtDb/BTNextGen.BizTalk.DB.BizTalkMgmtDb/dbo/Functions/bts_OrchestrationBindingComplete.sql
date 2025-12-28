CREATE FUNCTION [dbo].[bts_OrchestrationBindingComplete](@nOrchestrationID int)
RETURNS int
AS
	BEGIN
		DECLARE @retval int
			select @retval = count(*) from bts_orchestration service
			right join bts_orchestration_port port on (port.nOrchestrationID = service.nID and port.nBindingOption <> 3 and port.bLink = 0)
			left join bts_orchestration_port_binding binding on binding.nOrcPortID = port.nID
			where service.nID = @nOrchestrationID and (binding.nSendPortID is null and binding.nReceivePortID is null and binding.nSpgID is null)
		RETURN(@retval)
	END
