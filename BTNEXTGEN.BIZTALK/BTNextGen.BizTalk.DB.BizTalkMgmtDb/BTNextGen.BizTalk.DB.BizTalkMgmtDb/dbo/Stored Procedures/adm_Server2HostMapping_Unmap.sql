CREATE PROCEDURE [dbo].[adm_Server2HostMapping_Unmap]
@ServerName nvarchar(63),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction
  
  -- Are there not uninstalled host instances?
  if exists ( select * 
    from 
     adm_Server,
     adm_Server2HostMapping,
     adm_Host,
     adm_HostInstance
     
    where      
     adm_Server.Name = @ServerName AND
     adm_Host.Name = @HostName AND
     adm_Server2HostMapping.HostId = adm_Host.Id AND
     adm_Server2HostMapping.ServerId = adm_Server.Id AND 
     adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id AND
     adm_HostInstance.ConfigurationState <> 5 --eAPP_INST_CONFIG_NOT_INSTALLED
   )
  begin
   set @ErrCode = 0xC0C02526 -- CIS_E_ADMIN_CORE_SVR_MSG_BOX_UNMAP_HAS_APP_INST
   goto exit_proc
  end


  --TDDS: get service ID before actually deleting host instance
  declare @SvcId as uniqueidentifier
  declare @HostInstanceId as int
  select @SvcId=HostInst.UniqueId, @HostInstanceId = HostInst.Id
  from 
   adm_Host Host,
   adm_Server2HostMapping Mapping,
   adm_HostInstance HostInst,
   adm_Server Svr
  where 
   Host.Name = @HostName and
   Mapping.HostId = Host.Id and
   HostInst.Svr2HostMappingId = Mapping.Id and
   Mapping.ServerId = Svr.Id and
   Svr.Name = @ServerName

  --TDDS: get Host Tracking
  declare @HostTracking as int
  select @HostTracking=Host.HostTracking
  from 
   adm_Host Host
  where 
   Host.Name = @HostName

  -- delete adm_HostInstanceSetting records
  delete 
      adm_HostInstanceSetting
  where
      adm_HostInstanceSetting.HostInstanceId = @HostInstanceId

  -- delete adm_HostInstance records
  delete
   adm_HostInstance
  from
   adm_Server,
   adm_Server2HostMapping,
   adm_Host
  where      
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_Server2HostMapping.ServerId = adm_Server.Id AND 
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id

  -- update adm_Server2HostMapping record
  update
   adm_Server2HostMapping
  set
   DateModified = GETUTCDATE(), 
   IsMapped = 0
  from
   adm_Server,
   adm_Host
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_Server2HostMapping.ServerId = adm_Server.Id 

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

  -- TDDS configuration (unregister service if host instance had been deleted succesfully)
  if ( @HostTracking <> 0 )
  begin
   declare @GroupName nvarchar(256)
   set @GroupName = dbo.adm_GetGroupName()

   exec @ErrCode = TDDS_UnregisterService @SvcId
   if ( @ErrCode <> 0 ) goto exit_proc
  end


exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback tran
  return @ErrCode
 end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server2HostMapping_Unmap] TO [BTS_ADMIN_USERS]
    AS [dbo];

