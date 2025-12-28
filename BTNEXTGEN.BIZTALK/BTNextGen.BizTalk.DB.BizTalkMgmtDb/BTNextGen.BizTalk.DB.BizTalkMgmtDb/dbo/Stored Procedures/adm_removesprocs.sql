CREATE PROCEDURE [dbo].[adm_removesprocs]
AS
 --///////////////////////////////////////////////////////////////////////////
 --// remove stored procedures
 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_SafeAddLinkedServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[bts_SafeAddLinkedServer]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_CleanupMgmtDB]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_CleanupMgmtDB]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_IsBTSAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_IsBTSAdmin]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_GetGroupStatistics]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[bts_GetGroupStatistics]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_GetGroupUUID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[bts_GetGroupUUID]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Group_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Group_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Group_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Group_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_SetAnalysisServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Group_SetAnalysisServer]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_ForceDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_ForceDelete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox_Internal_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MessageBox_Internal_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server_Enum]

 -- if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 -- drop procedure [dbo].[adm_Server2HostMapping_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_Map]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_Map]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_Unmap]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_Unmap]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_ForceUnmap]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_ForceUnmap]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_PrepareHostInstState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_PrepareHostInstState]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Server2HostMapping_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_PrepareHostInstState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_PrepareHostInstState]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Delete]

 -- Removing Host ForceDelete functionality (EBiz Suite bug 6584)
 -- if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_ForceDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 -- drop procedure [dbo].[adm_Host_ForceDelete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Verify_Before_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Verify_Before_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_PrepareUpdateDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_PrepareUpdateDelete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_PrepareHostInstState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_HostInstance_PrepareHostInstState]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Adapter_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Adapter_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Adapter_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Adapter_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Adapter_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveHandler_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveHandler_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveHandler_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveHandler_Delete]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveHandler_Enum]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler_Create]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler_Update]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler_Enum]
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler2_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler2_Create]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler2_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler2_Update]
    
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler2_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler2_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler2_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler2_Enum]
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler2_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendHandler2_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendPort_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendPort_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendPort_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendPort_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendPort_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_SendPort_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_removesprocs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_removesprocs]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MsgBoxConfig_Subscriber_Create]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MsgBoxConfig_Subscriber_Create]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MsgBoxConfig_Subscriber_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_MsgBoxConfig_Subscriber_Delete]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Repl_Get_DistributionDB_Name]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_Repl_Get_DistributionDB_Name]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Queue_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Queue_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Queue_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Queue_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_Load]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_Enum]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_Update]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_Enlistment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_Enlistment]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_GetPortLrpBinding]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_GetPortLrpBinding]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_ToggleRLs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_ToggleRLs]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Orchestration_QueryDependencyInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Orchestration_QueryDependencyInfo]

 -- if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_util_MasterSubDB_Lookup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 -- drop procedure [dbo].[adm_util_MasterSubDB_Lookup]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ServiceInfo_Lookup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ServiceInfo_Lookup]
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocationOrchestration_Load]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveLocationOrchestration_Load]
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocationOrchestration_Enum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_ReceiveLocationOrchestration_Enum]

    if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_getSettingsForInProcHostInstance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_getSettingsForInProcHostInstance]
 
   if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_getSettingsForIsolatedHostInstance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_getSettingsForIsolatedHostInstance]

 --///////////////////////////////////////////////////////////////////////////
 --// remove TDDS related stored procedures

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host_Register_TDDS_Services]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_Host_Register_TDDS_Services]
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_AcquireAppLock]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[adm_AcquireAppLock]

 --///////////////////////////////////////////////////////////////////////////
 --// Remove Ops OM related sprocs
 
 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ops_MapPredicatePropIDToString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[ops_MapPredicatePropIDToString]

 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ops_LoadOrchestrationServiceNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[ops_LoadOrchestrationServiceNames]

 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ops_LoadSendPortServiceNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[ops_LoadSendPortServiceNames]

 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ops_LoadReceivePortServiceNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[ops_LoadReceivePortServiceNames]

 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ops_LoadSendPortSpecificFields]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[ops_LoadSendPortSpecificFields]

 --///////////////////////////////////////////////////////////////////////////
 --// This SP is used for looking up parties from the run-time
 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[admsvr_GetPartyByAliasNameValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[admsvr_GetPartyByAliasNameValue]

 --///////////////////////////////////////////////////////////////////////////
 --// remove functions
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_CheckRowCount]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_CheckRowCount]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetNumMisconfiguredRL]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_GetNumMisconfiguredRL]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetNumTransportConflictsInOrg]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_GetNumTransportConflictsInOrg]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetOrchestrationDependencies]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 0)
 drop function [dbo].[adm_GetOrchestrationDependencies]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetGroupName]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_GetGroupName]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetGroupId]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_GetGroupId]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GetNumInstalledHostInstances]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_GetNumInstalledHostInstances]
 
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_fnConvertLocalToUTCDate]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_fnConvertLocalToUTCDate]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_fnConvertUTCToLocalDate]') and OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
 drop function [dbo].[adm_fnConvertUTCToLocalDate]

 --///////////////////////////////////////////////////////////////////////////
 --// remove backup procs
 
 if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_MarkAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[sp_MarkAll]

 if exists (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_MarkBTSLogs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[sp_MarkBTSLogs]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BackupAllFull_Schedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[sp_BackupAllFull_Schedule]

 if exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BackupAllFull]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[sp_BackupAllFull]

 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BackupAllFull_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_BackupAllFull_Schedule]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BackupAllFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_BackupAllFull]
  
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_MarkAll]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_MarkAll]

 IF exists (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_MarkBTSLogs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_MarkBTSLogs]

 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BuildFullMarkName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
 drop procedure [dbo].[sp_BuildFullMarkName]

 IF exists (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_CleanMarkName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_CleanMarkName]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BlockTDDS]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_BlockTDDS]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BuildLogMarkString]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_BuildLogMarkString]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_BuildLogBackupString]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[sp_BuildLogBackupString]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_ForceFullBackup]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_ForceFullBackup]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_ReleaseBackupWriterLock]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_ReleaseBackupWriterLock]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_AcquireBackupWriterLock]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_AcquireBackupWriterLock]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_GetLinkedServerQTimeout]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetLinkedServerQTimeout]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_GetNextBackupSetId]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetNextBackupSetId]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_GetFileNameFromFilePath]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetFileNameFromFilePath]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_DeleteBackupHistory]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_DeleteBackupHistory]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_GetBackupHistory]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetBackupHistory]
 
 IF exists (select * from sysobjects where id = object_id(N'[dbo].[sp_GetBackupDatabasesForServer]' ) AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetBackupDatabasesForServer]
  
 IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_GetRemoteServerName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetRemoteServerName]

 IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_GetBackupDatabaseLocation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_GetBackupDatabaseLocation]
  
 IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sp_SetBackupCompression]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[sp_SetBackupCompression]

 IF EXISTS (SELECT * FROM sysobjects where id = object_id(N'[dbo].[sp_IsSSODB]') AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
  DROP FUNCTION [dbo].[sp_IsSSODB]
 
 --///////////////////////////////////////////////////////////////////////////
 --// Remove BAS related stored procedures

 IF exists (select * FROM sysobjects where id = object_id(N'[dbo].[bas_DeleteProperty]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[bas_DeleteProperty]

 IF exists (select * FROM sysobjects where id = object_id(N'[dbo].[bas_GetPropertyValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[bas_GetPropertyValue]

 IF exists (select * FROM sysobjects where id = object_id(N'[dbo].[bas_SetPropertyValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[bas_SetPropertyValue]

 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[adm_toggleDefaultAppFlag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[adm_toggleDefaultAppFlag]