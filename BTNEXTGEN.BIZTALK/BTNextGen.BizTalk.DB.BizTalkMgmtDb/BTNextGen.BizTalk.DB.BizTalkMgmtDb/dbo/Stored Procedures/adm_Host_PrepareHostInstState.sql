CREATE PROCEDURE [dbo].[adm_Host_PrepareHostInstState]
@Name nvarchar(80),
@NewState int -- extected to be 3 (eAPP_INST_CONFIG_UNINSTALL_FAIL)
AS
 set nocount on
 set xact_abort on
 
 declare @ErrCode as int, @LastError as int
 SELECT @ErrCode = 0, @LastError = 0

 begin transaction

  -- Trying to delete host? Nothing else is expected here.
  if(3 <> @NewState) -- eAPP_INST_CONFIG_UNINSTALL_FAIL
  begin
   set @ErrCode=0xC0C02402 -- CIS_E_INTERNAL_FAILURE
   goto exit_proc
  end
  
  -- Check if this host can be deleted
  exec @ErrCode = adm_Host_Verify_Before_Delete @Name
  select @LastError=@@error
  if(@LastError <> 0 AND @ErrCode =0 )
   set @ErrCode=@LastError
  
  if(@ErrCode <> 0)
   goto exit_proc
  
  -- Change HostInst configuration state
  -- Rule 1: when HostInst is not installed, don't change it's state to eAPP_INST_CONFIG_UNINSTALL_FAIL
  update
   HostInst
  set
   ConfigurationState=@NewState,
   DateModified = GETUTCDATE()
  from
   adm_HostInstance as HostInst
   join adm_Server2HostMapping as hostMap on hostMap.Id=HostInst.Svr2HostMappingId
   join adm_Host as Host on Host.Name = @Name
    AND Host.Id=hostMap.HostId
    AND (HostInst.ConfigurationState <> 5 --eAPP_INST_CONFIG_NOT_INSTALLED
      OR 3 <> @NewState) -- eAPP_INST_CONFIG_UNINSTALL_FAIL

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
    ON OBJECT::[dbo].[adm_Host_PrepareHostInstState] TO [BTS_ADMIN_USERS]
    AS [dbo];

