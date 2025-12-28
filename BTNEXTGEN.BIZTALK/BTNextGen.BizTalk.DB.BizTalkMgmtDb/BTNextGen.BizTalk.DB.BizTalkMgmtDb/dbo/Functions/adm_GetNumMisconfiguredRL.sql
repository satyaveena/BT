CREATE FUNCTION [dbo].[adm_GetNumMisconfiguredRL] (@ServiceGUID uniqueidentifier)
RETURNS int
AS
BEGIN
 return
  (select
   count(*)
  from
   bts_orchestration svc,
   bts_orchestration_port svcPort,
   bts_orchestration_port_binding portBinding,
   bts_receiveport lrp,
   adm_ReceiveLocation rl,
   adm_ReceiveHandler rh
  where
   svc.uidGUID = @ServiceGUID AND
   svc.nID = svcPort.nOrchestrationID AND
   svcPort.nID = portBinding.nOrcPortID AND
   portBinding.nReceivePortID = lrp.nID AND
   lrp.nID = rl.ReceivePortId AND
   rl.ReceiveHandlerId IS NULL) -- RL is not associated with any receive handler
END