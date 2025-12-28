CREATE PROCEDURE [dbo].[adm_HostInstance_Enum]
AS
 set nocount on
 set xact_abort on
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

 (select
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
  adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
  AND adm_Host.Id = adm_Server2HostMapping.HostId
  AND adm_Server.Id = adm_Server2HostMapping.ServerId
  AND #TempDB.HostInstanceId = adm_HostInstance.Id)
union
 -- This Union adds an additional row to represent all Host Instances belonging to a Clustered Host.
 -- When the Host Instances are clustered (running in an MSCS cluster environment), the UI needs to 
 -- show one instance instead of all the clustered instances.
 -- To do this the UI can query for ClusterInstanceType=2
 --
 -- Note: Valid values for adm_HostInstance.ClusterInstanceType are
 --            0 - Unclustered Host Instance
 --            1 - Clustered Host Instance
 --            2 - Clustered Host Instance that is active, this is determined by WMI
 --            3 - Virtual representation of a set of clustered Host Instances
 (select 
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
  '', -- blank out LoginName,
  '', -- blank out InstallationContext,
  adm_Host.HostTracking,
  3, -- to represent a virtual instance
  '',
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
  adm_Host.ClusterResourceGroupName IS NOT NULL
  AND adm_Host.ClusterResourceGroupName <> ''
  AND adm_Server2HostMapping.Id = adm_HostInstance.Svr2HostMappingId
  AND adm_Host.Id = adm_Server2HostMapping.HostId
  AND adm_Server.Id = adm_Server2HostMapping.ServerId
  AND adm_HostInstance.Id = 
     (
      select top 1 
       ahi2.Id 
      from
       adm_HostInstance ahi2, 
       adm_Host ah2, 
       adm_Server2HostMapping ashm2, 
       adm_Server as2
      where
       ashm2.Id = ahi2.Svr2HostMappingId
       AND ah2.Id = ashm2.HostId
       AND as2.Id = ashm2.ServerId
       AND adm_Host.Name = ah2.Name
     )
  AND adm_HostInstance.Id = #TempDB.HostInstanceId
 )
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_HostInstance_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_HostInstance_Enum] TO [BTS_OPERATORS]
    AS [dbo];

