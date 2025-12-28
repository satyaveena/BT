CREATE PROCEDURE [dbo].[adm_ReceiveLocationOrchestration_Load]
@RcvLocName nvarchar(256),
@OrchestrationName nvarchar(256),
@OrchestrationAssemblyName nvarchar(256),
@OrchestrationAssemblyVersion nvarchar(256),
@OrchestrationAssemblyCulture nvarchar(256),
@OrchestrationAssemblyPKToken nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 select @ErrCode = 0

 select
  rl.Name,
  orch.nvcFullName,
  asm.nvcName,
  asm.nvcVersion,
  asm.nvcCulture,
  asm.nvcPublicKeyToken,
  rl.InboundTransportURL,
  adapter.Name,
  transpHost.Name,
  bts_pipeline.FullyQualifiedName, --@ReceiveServiceName 
  lrp.nvcName, --@ReceivePortName
  rl.Disabled,
  rl.IsPrimary,
  svcHost.Name,
  case -- Mapping deployment state to admin WMI state
   when (orch.nOrchestrationStatus = 1 AND dbo.bts_OrchestrationBindingComplete(orch.nID) > 0) then 1 -- eSvcStatusUnbound
   when (orch.nOrchestrationStatus = 1 AND dbo.bts_OrchestrationBindingComplete(orch.nID) = 0) then 2 -- eSvcStatusBound
   when orch.nOrchestrationStatus = 2 then 3  -- eSvcStatusEnlisted
   when orch.nOrchestrationStatus = 3 then 4  -- eSvcStatusStarted
  end,
  case
   when orch.dtModified > rl.DateModified then orch.dtModified
   else rl.DateModified
  end
 from
  bts_assembly asm,
  bts_orchestration orch left outer join adm_Host svcHost on orch.nAdminHostID = svcHost.Id
  join bts_orchestration_port svcPort on orch.nID = svcPort.nOrchestrationID
  join bts_orchestration_port_binding portBinding on svcPort.nID = portBinding.nOrcPortID
  join bts_receiveport lrp on portBinding.nReceivePortID = lrp.nID
  join adm_ReceiveLocation rl on  lrp.nID = rl.ReceivePortId
  left outer join adm_ReceiveHandler rh on rh.Id = rl.ReceiveHandlerId
  left outer join adm_Host transpHost on rh.HostId = transpHost.Id,
  adm_Adapter adapter,
  bts_pipeline
 where
  orch.nAssemblyID = asm.nID AND
  orch.nvcFullName = @OrchestrationName AND
  adapter.Id = rl.AdapterId AND
  rl.ReceivePipelineId = bts_pipeline.Id AND
  asm.nvcName = @OrchestrationAssemblyName AND
  asm.nvcVersion = @OrchestrationAssemblyVersion AND
  asm.nvcCulture = @OrchestrationAssemblyCulture AND
  asm.nvcPublicKeyToken = @OrchestrationAssemblyPKToken AND
  rl.Name = @RcvLocName

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) goto exit_proc


exit_proc:
 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveLocationOrchestration_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveLocationOrchestration_Load] TO [BTS_OPERATORS]
    AS [dbo];

