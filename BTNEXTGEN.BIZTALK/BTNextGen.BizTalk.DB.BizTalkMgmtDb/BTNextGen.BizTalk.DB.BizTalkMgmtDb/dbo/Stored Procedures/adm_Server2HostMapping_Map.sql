CREATE PROCEDURE [dbo].[adm_Server2HostMapping_Map]
@ServerName nvarchar(63),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 declare @HostInstanceId as int
 select @ErrCode = 0

 begin transaction

  -- Disallow inserts for clustered hosts
  if exists (
    select * 
    from 
     adm_Host
    where
     adm_Host.Name = @HostName AND
     adm_Host.ClusterResourceGroupName IS NOT NULL AND 
     adm_Host.ClusterResourceGroupName <> ''
   )
  begin
   set @ErrCode = 0xC0C024CB -- CIS_E_ADMIN_OPERATION_NOT_ALLOWED_ON_CLUSTERED_HOST
   goto exit_proc
  end
  
  insert into adm_HostInstance 
   (Svr2HostMappingId,
    Name, 
    LoginName, 
    DisableHostInstance,
    ConfigurationState)
  select
    adm_Server2HostMapping.Id,
    N'Microsoft BizTalk Server '+ adm_Host.Name + N' ' + adm_Server.Name,
   adm_Host.LastUsedLogon,
   0, -- DisableHostInstance = "false"
   5  -- eAPP_INST_CONFIG_NOT_INSTALLED
  from  
   adm_Server,
   adm_Server2HostMapping,
   adm_Host
  where
   adm_Server.Name = @ServerName AND
   adm_Host.Name = @HostName AND
   adm_Server2HostMapping.HostId = adm_Host.Id AND
   adm_Server2HostMapping.ServerId = adm_Server.Id  
   
  set @HostInstanceId = @@IDENTITY  
  -- only one instance should be updated
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  
  insert into adm_HostInstanceSetting (HostInstanceId,PropertyName, PropertyValue)
  select @HostInstanceId, N'CLRMaxIOThreads', N'250'
  union all
  select @HostInstanceId, N'CLRMinIOThreads', N'25'
  union all
  select @HostInstanceId, N'CLRMaxWorkerThreads', N'25'
  union all
  select @HostInstanceId, N'CLRMinWorkerThreads', N'5'
  union all
  select @HostInstanceId, N'PhysicalMemoryMaximalUsage', N'85'
  union all
  select @HostInstanceId, N'PhysicalMemoryOptimalUsage', N'70'
  union all
  select @HostInstanceId, N'VirtualMemoryMaximalUsage', N'85'
  union all
  select @HostInstanceId, N'VirtualMemoryOptimalUsage', N'65'
  
  update adm_Server2HostMapping
  set
   DateModified = GETUTCDATE(), 
   IsMapped = -1
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

  if (dbo.adm_GetNumTransportConflictsInOrg() <> 0)
   set @ErrCode = 0xC0C0257D -- CIS_E_ADMIN_CANNOT_MAP_BECAUSE_TRANSPORT_CONSTRAINT_PRIVATE_ERROR

  if ( @ErrCode <> 0 ) goto exit_proc

 -- TDDS configuration (register service)
  declare @HostTracking as int
  select @HostTracking=Host.HostTracking
  from 
   adm_Host Host
  where 
   Host.Name = @HostName
   
  if ( @HostTracking <> 0 )
  begin
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

   declare @GroupName nvarchar(256)
   set @GroupName = dbo.adm_GetGroupName()

   exec @ErrCode = TDDS_RegisterService @SvcId, @ServerName
   if ( @ErrCode <> 0 ) goto exit_proc
  end

 
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
    ON OBJECT::[dbo].[adm_Server2HostMapping_Map] TO [BTS_ADMIN_USERS]
    AS [dbo];

