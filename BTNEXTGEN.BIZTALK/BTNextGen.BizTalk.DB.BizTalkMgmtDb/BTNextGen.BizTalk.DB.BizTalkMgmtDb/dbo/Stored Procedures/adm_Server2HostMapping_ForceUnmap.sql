CREATE PROCEDURE [dbo].[adm_Server2HostMapping_ForceUnmap]
@ServerName nvarchar(63),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction
 
  --TDDS: get service ID before actually deleting host instance
  declare @SvcId as uniqueidentifier
  select @SvcId=HostInst.UniqueId
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


 
  -- Check if already tried to delete host instances?
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
     NOT (adm_HostInstance.ConfigurationState = 3 -- eAPP_INST_CONFIG_UNINSTALL_FAIL
       OR adm_HostInstance.ConfigurationState = 5) -- eAPP_INST_CONFIG_NOT_INSTALLED
   )
  begin
   set @ErrCode = 0xC0C02527 -- CIS_E_ADMIN_CORE_SVR_MSG_BOX_FORCEUNMAP_BEFORE_APP_INST_DELETE
   goto exit_proc
  end

  insert into adm_HostInstanceZombie
   (Name, GroupName, HostName, ServerName, 
   NTGroupName, LoginName, UniqueId)
  select adm_HostInstance.Name, dbo.adm_GetGroupName(), adm_Host.Name, adm_Server.Name, 
   adm_Host.NTGroupName, adm_HostInstance.LoginName, adm_HostInstance.UniqueId
  from
    adm_Server,
   adm_Server2HostMapping,
   adm_Host,
   adm_HostInstance
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.ServerId = adm_Server.Id AND 
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id AND
   adm_HostInstance.ConfigurationState = 3 -- eAPP_INST_CONFIG_UNINSTALL_FAIL

          -- delete adm_HostInstanceSetting records
  delete 
      adm_HostInstanceSetting
  from
      adm_Server,
      adm_Server2HostMapping,
      adm_Host,
      adm_HostInstance          
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.ServerId = adm_Server.Id AND 
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id AND
   adm_HostInstanceSetting.HostInstanceId = adm_HostInstance.Id

  delete
   adm_HostInstance
  from
    adm_Server,
   adm_Server2HostMapping,
   adm_Host
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.ServerId = adm_Server.Id AND 
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id

  update adm_Server2HostMapping
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

  -- If the last host instance was unmapped, clear the ClusterResourceGroupName property of the host to indicate this host is no longer clustered
  if not exists (  select * from
        adm_HostInstance,
         adm_Server,
        adm_Server2HostMapping,
        adm_Host
       where
        adm_Host.Name = @HostName AND
        adm_Server2HostMapping.ServerId = adm_Server.Id AND 
        adm_Server2HostMapping.HostId = adm_Host.Id AND
        adm_HostInstance.Svr2HostMappingId=adm_Server2HostMapping.Id
     )
  begin
   update adm_Host
   set
    ClusterResourceGroupName = ''
   where
    adm_Host.Name = @HostName
   set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
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
    ON OBJECT::[dbo].[adm_Server2HostMapping_ForceUnmap] TO [BTS_ADMIN_USERS]
    AS [dbo];

