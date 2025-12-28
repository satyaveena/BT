CREATE PROCEDURE [dbo].[adm_Orchestration_ToggleRLs]
@ServiceGUID uniqueidentifier,
@Disabled int
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @bHasOpenedTransaction as int
 select @ErrCode = 0, @bHasOpenedTransaction=0

 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  set @bHasOpenedTransaction=1
 end

  -- Verify that all receive locations are properly configured during Enabling (only)
  if ( @Disabled = 0 AND dbo.adm_GetNumMisconfiguredRL(@ServiceGUID) > 0 )
  begin
   set @ErrCode = 0xC0C02573 -- CIS_E_ADMIN_SVC_ENLIST_RL_MISCONFIGURED
   goto exit_proc
  end

  -- Toggle all receive locations associated with the specified service
  update
   adm_ReceiveLocation
  set
   Disabled = @Disabled
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
   rl.ReceiveHandlerId IS NOT NULL  -- only enable RLs which are hosted by receive handler

exit_proc:
 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
  begin
   rollback transaction
   return @ErrCode
  end
 end

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_ToggleRLs] TO [BTS_ADMIN_USERS]
    AS [dbo];

