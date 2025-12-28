CREATE PROCEDURE [dbo].[adm_Orchestration_Enlistment]
@ServiceGUID uniqueidentifier,
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int, @bHasOpenedTransaction as int, @OrchestrationID as int, @HostId as int, @HostType as int
 select @ErrCode = 0, @bHasOpenedTransaction=0, @OrchestrationID = 0, @HostId = 0, @HostType = 0

 if(@@trancount = 0) -- this sp should be called from DTC
 begin
  begin transaction
  set @bHasOpenedTransaction=1
 end

  -- Resolve the OrchestrationID
  select
   @OrchestrationID = nID
  from
   bts_orchestration
  where
   uidGUID = @ServiceGUID

  -- Check whether load was successful...
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- Resolve the Host ID
  select @HostId = Id, @HostType = HostType
  from adm_Host
  where Name = @HostName

  if ( @@ROWCOUNT < 1 )
  begin
   set @ErrCode = 0xC0C02571 -- CIS_E_ADMIN_SVC_ENLIST_INVALID_HOST
   goto exit_proc
  end
  
  -- Check that the specified host is creatable
  if ( @HostType <> 1 ) -- eHostTypeInProcess
  begin
   set @ErrCode = 0xC0C025C5 -- CIS_E_ADMIN_SVC_ENLIST_INVALID_HOST_TYPE
   goto exit_proc
  end
 
  -- 
  -- Invoke Partner Mgmt stored function to check whether Service is fully bound or not
  --
  if ( dbo.bts_OrchestrationBindingComplete(@OrchestrationID) <> 0 )
  begin
   set @ErrCode = 0xC0C02572 -- CIS_E_ADMIN_SVC_ENLIST_NOT_BOUND
   goto exit_proc
  end

  -- Check that there must be at least one MsgBox in the Group before any service can be enlisted
  select Id from adm_MessageBox
   
  if ( @@ROWCOUNT < 1 )
  begin
   set @ErrCode = 0xC0C02577 -- CIS_E_ADMIN_CORE_ZERO_MSGBOX_ENLISTMENT
   goto exit_proc
  end

  -- Check whether all the related RLs are fully configured
  if ( dbo.adm_GetNumMisconfiguredRL(@ServiceGUID) > 0 )
  begin
   set @ErrCode = 0xC0C02573 -- CIS_E_ADMIN_SVC_ENLIST_RL_MISCONFIGURED
   goto exit_proc
  end
  
  -- Update the enlisting Service with the hosting Host's ID
  update
   bts_orchestration
  set
   nAdminHostID = @HostId
  where
   nID = @OrchestrationID

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
    ON OBJECT::[dbo].[adm_Orchestration_Enlistment] TO [BTS_ADMIN_USERS]
    AS [dbo];

