CREATE PROCEDURE [dbo].[adm_Orchestration_Load]
@Name nvarchar(256),
@AssemblyName nvarchar(256),
@AssemblyVersion nvarchar(256),
@AssemblyCulture nvarchar(256),
@AssemblyPKToken nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

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
  orch.nAssemblyID = asm.nID AND
  orch.nvcFullName = @Name AND
  asm.nvcName = @AssemblyName AND
  asm.nvcVersion = @AssemblyVersion AND
  asm.nvcCulture = @AssemblyCulture AND
  asm.nvcPublicKeyToken = @AssemblyPKToken

 -- Check whehter load was successful...
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_Load] TO [BTS_OPERATORS]
    AS [dbo];

