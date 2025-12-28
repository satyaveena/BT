CREATE PROCEDURE [dbo].[bts_GetGroupStatistics]
 @nHostInstanceId uniqueidentifier,
 @nApplications int output,
 @nSendPorts int output,
 @nDynamicSendPorts int output,
 @nOrderedSendPorts int output,
 @nSendPortsWithServiceWindow int output,
 @nSendPortsWithBackupTransport int output,
 @nReceiveLocations int output,
 @nReceiveLocationsWithServiceWindow int output,
 @nOrchestrations int output,
 @nSchemas int output,
 @nMaps int output,
 @nPipelines int output,
 @nMachines int output,
 @nHosts int output,
 @n32bitHosts int output,
 @nHostInstances int output,
 @nMessageBoxes int output
AS
 declare @nLowestHostInstanceId uniqueidentifier

 --check if @nHostId is the lowest host id
 select top 1 @nLowestHostInstanceId = [dbo].[adm_HostInstance].UniqueId from [dbo].[adm_HostInstance] with (NOLOCK)
  join [dbo].[adm_Server2HostMapping] with (NOLOCK) on [dbo].[adm_HostInstance].Svr2HostMappingId = [dbo].[adm_Server2HostMapping].Id
  join [dbo].[adm_Host] with (NOLOCK) on [dbo].[adm_Server2HostMapping].HostId = [dbo].[adm_Host].Id
  where [dbo].[adm_Host].HostType = 1 order by [dbo].[adm_HostInstance].Id asc
 --only proceed if the host instance passed is the one with the lowest id
 if (@nHostInstanceId != @nLowestHostInstanceId)
  return -1

 --application count
 select @nApplications = COUNT(*) - 1 from [dbo].[bts_application] with (NOLOCK)

 --send port count
 select @nSendPorts = COUNT(*) from [dbo].[bts_sendport] with (NOLOCK)

 --dynamic send port count
 select @nDynamicSendPorts = COUNT(*) from [dbo].[bts_sendport] with (NOLOCK)
  where [dbo].[bts_sendport].bDynamic = 1

 --ordered send port count
 select @nOrderedSendPorts = COUNT(distinct([dbo].[bts_sendport_transport].nSendPortID)) from [dbo].[bts_sendport_transport] with (NOLOCK)
  where [dbo].[bts_sendport_transport].bOrderedDelivery = 1

 --count of send ports with service window
 select @nSendPortsWithServiceWindow = COUNT(distinct([dbo].[bts_sendport_transport].nSendPortID)) from [dbo].[bts_sendport_transport] with (NOLOCK)
  where [dbo].[bts_sendport_transport].bIsServiceWindow = 1

 --count of send ports with backup transport
 select @nSendPortsWithBackupTransport = COUNT(*) from [dbo].[bts_sendport_transport] with (NOLOCK)
  where [dbo].[bts_sendport_transport].bIsPrimary = 0 and
  [dbo].[bts_sendport_transport].nSendHandlerID is not NULL and
  [dbo].[bts_sendport_transport].nTransportTypeId is not NULL

 --receive location count
 select @nReceiveLocations = COUNT(*) from [dbo].[adm_ReceiveLocation] with (NOLOCK)

 --count of receive location with service window
 select @nReceiveLocationsWithServiceWindow = COUNT(*) from [dbo].[adm_ReceiveLocation] with (NOLOCK)
  where [dbo].[adm_ReceiveLocation].OperatingWindowEnabled = 1

 --orchestration count
 select @nOrchestrations = COUNT(*) from [dbo].[bts_orchestration] with (NOLOCK)

 --schema count (only user deployed schemas)
 select @nSchemas = COUNT(distinct([dbo].[bt_DocumentSpec].itemid)) from [dbo].[bt_DocumentSpec] with (NOLOCK)
  inner join [dbo].[bts_item] with (NOLOCK) on [dbo].[bt_DocumentSpec].itemid = [dbo].[bts_item].id
  left join [dbo].[bt_XMLShare] with (NOLOCK) on [dbo].[bt_DocumentSpec].shareid = [dbo].[bt_XMLShare].id
  inner join [dbo].[bts_assembly] with (NOLOCK) on [dbo].[bt_DocumentSpec].assemblyid = [dbo].[bts_assembly].nID
  where [dbo].[bts_assembly].nSystemAssembly = 0

 --map count
 select @nMaps = COUNT(*) from [dbo].[bt_MapSpec] with (NOLOCK)
  inner join [dbo].[bts_item] with (NOLOCK) on [dbo].[bt_MapSpec].itemid = [dbo].[bts_item].id

 --pipeline count (only user deployed pipelines)
 select @nPipelines = COUNT(*) from [dbo].[bts_pipeline] with (NOLOCK)
  join [dbo].[bts_assembly] with (NOLOCK) on [dbo].[bts_pipeline].nAssemblyID = [dbo].[bts_assembly].nID
  where [dbo].[bts_assembly].nSystemAssembly = 0

 --runtime machine count
 select @nMachines = COUNT(*) from [dbo].[adm_Server] with (NOLOCK)

 --host count
 select @nHosts = COUNT(*) from [dbo].[adm_Host] with (NOLOCK)

 --32 bit host count
 select @n32bitHosts = COUNT(*) from [dbo].[adm_Host] with (NOLOCK)
  where [dbo].[adm_Host].IsHost32BitOnly = 1

 --host instance count
 select @nHostInstances = COUNT(*) from [dbo].[adm_HostInstance] with (NOLOCK)

 --message box count
 select @nMessageBoxes = COUNT(*) from [dbo].[adm_MessageBox] with (NOLOCK)

 return 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetGroupStatistics] TO [BTS_HOST_USERS]
    AS [dbo];

