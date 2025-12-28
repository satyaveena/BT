CREATE PROCEDURE [dbo].[adm_Host_Delete]
@Name nvarchar(80)
AS
 set nocount on
 set xact_abort on
 declare @bHasOpenedTransaction as int, @ErrCode as int, @LastError as int, @HostId as int
 select @ErrCode = 0, @LastError=0, @HostId=0
 
 if(@@trancount = 0)
 begin
  begin transaction
  set @bHasOpenedTransaction=1
 end
 else
 begin
  set @bHasOpenedTransaction=0
 end
  -- Resolve foreign key: GroupId, HostId
  select @HostId = Id from adm_Host where Name = @Name
  -- Check if this host can be deleted
  exec @ErrCode = adm_Host_Verify_Before_Delete @Name
  select @LastError=@@error
  if(@LastError <> 0 AND @ErrCode =0 )
   set @ErrCode=@LastError
  
  if(@ErrCode <> 0)
   goto exit_proc
  -- Are there not uninstalled host instances?
  if exists (select *
     from adm_HostInstance HostInst,
      adm_Host Host,
       adm_Server2HostMapping hostMap
     where
      Host.Name = @Name AND
      hostMap.HostId = Host.Id AND
      hostMap.Id = HostInst.Svr2HostMappingId AND
      HostInst.ConfigurationState <> 5  -- eAPP_INST_CONFIG_NOT_INSTALLED
     )
  begin
   set @ErrCode = 0xC0C0251A -- CIS_E_ADMIN_CORE_APP_TYPE_DELETE_HAS_APP_INST
   goto exit_proc
  end
  
  -- delete adm_HostInstanceSetting records
  delete 
      adm_HostInstanceSetting
  from
      adm_Server2HostMapping,
      adm_Host,
      adm_HostInstance          
  where
   adm_Host.Name = @Name AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id AND
   adm_HostInstanceSetting.HostInstanceId = adm_HostInstance.Id

  -- Delete related adm_HostInstance records
  delete
   adm_HostInstance
  from 
   adm_Host,
   adm_Server2HostMapping
  where 
   adm_Host.Name = @Name AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_HostInstance.Svr2HostMappingId = adm_Server2HostMapping.Id
  -- Delete related adm_Server2HostMapping records
  delete 
   adm_Server2HostMapping
  from
   adm_Host
  where
   adm_Host.Name = @Name AND
   adm_Server2HostMapping.HostId = adm_Host.Id
  
  -- Delete the setting from adm_HostSetting
  delete
      adm_HostSetting
  from
      adm_Host
  where
      adm_Host.Name = @Name AND
      adm_HostSetting.HostId = adm_Host.Id
      
  -- Delete related adm_Host records
  delete 
   adm_Host
  where
   adm_Host.Name = @Name
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
exit_proc:
 if(@ErrCode = 0 AND 0 <> @bHasOpenedTransaction)
  commit transaction
 else if (@ErrCode <> 0)
 begin
  if(1 = @bHasOpenedTransaction)
   rollback tran
  return @ErrCode
 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

