CREATE PROCEDURE [dbo].[adm_HostInstance_Load]
@Name nvarchar(256)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 set @ErrCode = 0
    (select HostInstanceId,
           MAX(case when PropertyName= N'CLRMaxWorkerThreads' then CAST(PropertyValue as int) END) as CLRMaxWorkerThreads,
           MAX(case when PropertyName= N'CLRMinWorkerThreads' then CAST(PropertyValue as int) END) as CLRMinWorkerThreads,
           MAX(case when PropertyName= N'CLRMaxIOThreads' then CAST(PropertyValue as int) END) as CLRMaxIOThreads,
           MAX(case when PropertyName= N'CLRMinIOThreads' then CAST(PropertyValue as int) END) as CLRMinIOThreads,
           MAX(case when PropertyName= N'PhysicalMemoryOptimalUsage' then CAST(PropertyValue as int) END) as PhysicalMemoryOptimalUsage,
           MAX(case when PropertyName= N'PhysicalMemoryMaximalUsage' then CAST(PropertyValue as int) END) as PhysicalMemoryMaximalUsage,
           MAX(case when PropertyName= N'VirtualMemoryOptimalUsage' then CAST(PropertyValue as int) END) as VirtualMemoryOptimalUsage,
           MAX(case when PropertyName= N'VirtualMemoryMaximalUsage' then CAST(PropertyValue as int) END) as VirtualMemoryMaximalUsage
    INTO #TempDB       
    From adm_HostInstanceSetting
    Group BY HostInstanceId)

 select
  adm_HostInstance.Id, 
  adm_HostInstance.Name, 
  adm_HostInstance.DateModified, 
  adm_Host.NTGroupName,
  adm_Host.Name,
  adm_Host.HostType,
  adm_HostInstance.UniqueId,
  adm_Server.Name,
  adm_HostInstance.DisableHostInstance,
  adm_HostInstance.ConfigurationState,
  adm_HostInstance.LoginName,
  adm_HostInstance.InstallationContext,
  adm_Host.HostTracking,
  case
   when (adm_Host.ClusterResourceGroupName IS NOT NULL AND adm_Host.ClusterResourceGroupName <> '') then 1 
   else 0 
  end as ClusterInstanceType,
  adm_HostInstance.nvcDescription,
  CLRMaxWorkerThreads,
  CLRMinWorkerThreads,
  CLRMaxIOThreads,
  CLRMinIOThreads,
  PhysicalMemoryOptimalUsage,
  PhysicalMemoryMaximalUsage,
  VirtualMemoryOptimalUsage,
  VirtualMemoryMaximalUsage
 from
  adm_HostInstance,
  adm_Host,
  adm_Server2HostMapping,
  adm_Server,
  #TempDB
 where
  adm_HostInstance.Name = @Name
  AND adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
  AND adm_Host.Id = adm_Server2HostMapping.HostId
  AND adm_Server.Id = adm_Server2HostMapping.ServerId
  AND #TempDB.HostInstanceId = adm_HostInstance.Id
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_HostInstance_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_HostInstance_Load] TO [BTS_OPERATORS]
    AS [dbo];

