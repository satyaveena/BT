CREATE PROCEDURE [dbo].[adm_Orchestration_QueryDependencyInfo]
@Direction nchar(1), -- 'U' upstream / 'D' downstream
@Name nvarchar(256),
@AssemblyName nvarchar(256),
@AssemblyVersion nvarchar(256),
@AssemblyCulture nvarchar(256),
@AssemblyPKToken nvarchar(256)
AS
 set nocount on

 declare @nOrchestrationID as int

 -- Lookup the service ID 
 select
  @nOrchestrationID = orch.nID
 from
  bts_assembly asm,
  bts_orchestration orch
 where
  orch.nAssemblyID = asm.nID AND
  orch.nvcFullName = @Name AND
  asm.nvcName = @AssemblyName AND
  asm.nvcVersion = @AssemblyVersion AND
  asm.nvcCulture = @AssemblyCulture AND
  asm.nvcPublicKeyToken = @AssemblyPKToken

 if ( @@ROWCOUNT = 0 )
  return
  
 -- Execute deployment stored procedure for retrieving relevant info from
 -- service dependency table
 select
  asm1.nvcName as CallerAssemblyName,
  asm1.nvcVersion as CallerAssemblyVersion,
  asm1.nvcCulture as CallerAssemblyCulture,
  asm1.nvcPublicKeyToken as CallerAssemblyPKToken,
  svc1.nvcFullName as CallerService,
  host1.Name as CallerEnlistedHost,
  asm2.nvcName as CalleeAssemblyName,
  asm2.nvcVersion as CalleeAssemblyVersion,
  asm2.nvcCulture as CalleeAssemblyCulture,
  asm2.nvcPublicKeyToken as CalleeAssemblyPKToken,
  svc2.nvcFullName as CalleeService,
  host2.Name as CalleeEnlistedHost,
  dp.CallType as CallType,
  svc1.nOrchestrationStatus as CallerServiceStatus,
  svc2.nOrchestrationStatus as CalleeServiceStatus,
  dp.Depth as Depth
 from
  adm_GetOrchestrationDependencies(@nOrchestrationID, @Direction) dp,
  bts_assembly asm1,
  bts_orchestration svc1 left outer join adm_Host host1 on svc1.nAdminHostID = host1.Id,
  bts_assembly asm2,
  bts_orchestration svc2 left outer join adm_Host host2 on svc2.nAdminHostID = host2.Id
 where
  dp.CallerSvcId = svc1.nID AND
  svc1.nAssemblyID = asm1.nID AND
  dp.CalleeSvcId = svc2.nID AND
  svc2.nAssemblyID = asm2.nID
 
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_QueryDependencyInfo] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_QueryDependencyInfo] TO [BTS_OPERATORS]
    AS [dbo];

