CREATE PROCEDURE [dbo].[adm_Orchestration_Enum]
AS
 set nocount on
 set xact_abort on

 select
  orch.nvcFullName,
  asm.nvcName,
  asm.nvcVersion,
  asm.nvcCulture,
  asm.nvcPublicKeyToken,
  orch.dtModified,
  host.Name,
  case -- Mapping deployment state to admin WMI state
   when (orch.nOrchestrationStatus = 1 AND dbo.bts_OrchestrationBindingComplete(orch.nID) > 0) then 1 -- eSvcStatusUnbound
   when (orch.nOrchestrationStatus = 1 AND dbo.bts_OrchestrationBindingComplete(orch.nID) = 0) then 2 -- eSvcStatusBound
   when orch.nOrchestrationStatus = 2 then 3  -- eSvcStatusEnlisted
   when orch.nOrchestrationStatus = 3 then 4  -- eSvcStatusStarted
  end
 from
  bts_assembly asm,
  bts_orchestration orch left outer join adm_Host host on orch.nAdminHostID = host.Id
 where
  orch.nAssemblyID = asm.nID

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_Enum] TO [BTS_OPERATORS]
    AS [dbo];

