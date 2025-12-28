CREATE PROCEDURE [dbo].[adm_Orchestration_GetPortLrpBinding]
@ServiceGUID uniqueidentifier
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 select @ErrCode = 0

 -- Retrieve the list of (ReceivePort, LRP) binding info
 select
        svcPort.uidGUID as portID,
        lrp.uidGUID as lrpID
 from
  bts_orchestration svc WITH (NOLOCK),
        bts_orchestration_port svcPort WITH (NOLOCK),
        bts_orchestration_port_binding binding WITH (NOLOCK),
        bts_receiveport lrp WITH (NOLOCK)
 where
  svc.uidGUID = @ServiceGUID AND
  svc.nID = svcPort.nOrchestrationID AND
        svcPort.nID = binding.nOrcPortID AND
        binding.nReceivePortID = lrp.nID --AND
        --svcPort.nPolarity = 1 -- inbound port only (confirm with Deployment)

exit_proc:

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_GetPortLrpBinding] TO [BTS_ADMIN_USERS]
    AS [dbo];

