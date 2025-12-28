CREATE PROCEDURE [dbo].[adm_Orchestration_Update]
@Name nvarchar(256),
@AssemblyName nvarchar(256),
@AssemblyVersion nvarchar(256),
@AssemblyCulture nvarchar(256),
@AssemblyPKToken nvarchar(256),
@ServiceStatus int
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  update
   bts_orchestration
  set
   nOrchestrationStatus =
    case -- Mapping admin WMI state to deployment state
     when (@ServiceStatus = 1 OR @ServiceStatus = 2) then 1 -- eSvcStatusUnbound or eSvcStatusBound
     when  @ServiceStatus = 3 then 2       -- eSvcStatusEnlisted
     when  @ServiceStatus = 4 then 3       -- eSvcStatusStarted
     when  @ServiceStatus = 5 then 4       -- eSvcStatusStopping
    end,
   nAdminHostID =
    case -- Clear AdminHostID when unenlisting
     when (@ServiceStatus = 1 OR @ServiceStatus = 2) then NULL -- eSvcStatusUnbound or eSvcStatusBound
     else nAdminHostID
    end
  from
   bts_assembly asm,
   bts_orchestration orch
  where
   orch.nvcFullName = @Name AND
   orch.nAssemblyID = asm.nID AND
   asm.nvcName = @AssemblyName AND
   asm.nvcVersion = @AssemblyVersion AND
   asm.nvcCulture = @AssemblyCulture AND
   asm.nvcPublicKeyToken = @AssemblyPKToken 

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Orchestration_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

