CREATE FUNCTION [dbo].[adm_CanReferenceBeDeleted]
(
	@appId int,
	@referenceAppId int
) RETURNS bit
AS
BEGIN
	if(IsNull(@appId, N'') = N'')
		return 1
	if(IsNull(@referenceAppId, N'') = N'')
		return 1
	-- check if a receive location in appId refers to a send pipeline in referenceAppId
	if exists( select 1 from adm_ReceiveLocation, bts_receiveport, bts_pipeline, bts_assembly
		where adm_ReceiveLocation.ReceivePortId = bts_receiveport.nID
		and bts_receiveport.nApplicationID = @appId
		and adm_ReceiveLocation.SendPipelineId = bts_pipeline.Id
		and bts_pipeline.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @referenceAppId )
		return 0
	-- check if a receive location in appId refers to a receive pipeline in referenceAppId
	if exists( select 1 from adm_ReceiveLocation, bts_receiveport, bts_pipeline, bts_assembly
		where adm_ReceiveLocation.ReceivePortId = bts_receiveport.nID
		and bts_receiveport.nApplicationID = @appId
		and adm_ReceiveLocation.ReceivePipelineId = bts_pipeline.Id
		and bts_pipeline.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @referenceAppId )
		return 0
	-- check if an orchestration in appId refers to a sendportgroup in referenceAppId
	if exists( select 1 from bts_orchestration, bts_orchestration_port, bts_orchestration_port_binding, bts_assembly, bts_sendportgroup
		where bts_orchestration.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @appId
		and bts_orchestration.nID = bts_orchestration_port.nOrchestrationID
		and bts_orchestration_port.nID = bts_orchestration_port_binding.nOrcPortID
		and bts_orchestration_port_binding.nSpgID = bts_sendportgroup.nID
		and bts_sendportgroup.nApplicationID = @referenceAppId )
		return 0
	-- check if an orchestration in appId refers to a sendport in referenceAppId
	if exists( select 1 from bts_orchestration, bts_orchestration_port, bts_orchestration_port_binding, bts_assembly, bts_sendport
		where bts_orchestration.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @appId
		and bts_orchestration.nID = bts_orchestration_port.nOrchestrationID
		and bts_orchestration_port.nID = bts_orchestration_port_binding.nOrcPortID
		and bts_orchestration_port_binding.nSendPortID = bts_sendport.nID
		and bts_sendport.nApplicationID = @referenceAppId )
		return 0
	-- check if an orchestration in appId refers to a receiveport in referenceAppId
	if exists( select 1 from bts_orchestration, bts_orchestration_port, bts_orchestration_port_binding, bts_assembly, bts_receiveport
		where bts_orchestration.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @appId
		and bts_orchestration.nID = bts_orchestration_port.nOrchestrationID
		and bts_orchestration_port.nID = bts_orchestration_port_binding.nOrcPortID
		and bts_orchestration_port_binding.nReceivePortID = bts_receiveport.nID
		and bts_receiveport.nApplicationID = @referenceAppId )
		return 0
	-- check if a receive port in appId refers to a transform in referenceAppId
	if exists( select 1 from bts_receiveport, bts_receiveport_transform, bt_MapSpec, bts_assembly
		where bts_receiveport.nApplicationID = @appId
		and bts_receiveport.nID = bts_receiveport_transform.nReceivePortID
		and bts_receiveport_transform.uidTransformGUID = bt_MapSpec.id
		and bt_MapSpec.assemblyid = bts_assembly.nID
		and bts_assembly.nApplicationID = @referenceAppId )
		return 0
	-- check if a sendportgroup in appId refers to a sendport in referenceAppId
	if exists( select 1 from bts_sendportgroup, bts_spg_sendport, bts_sendport
		where bts_sendportgroup.nApplicationID = @appId
		and bts_sendportgroup.nID = bts_spg_sendport.nSendPortGroupID
		and bts_spg_sendport.nSendPortID = bts_sendport.nID
		and bts_sendport.nApplicationID = @referenceAppId )
		return 0
	-- check if an send port in appId refers to a transform in referenceAppId
	if exists( select 1 from bts_sendport, bts_sendport_transform, bt_MapSpec, bts_assembly
		where bts_sendport.nApplicationID = @appId
		and bts_sendport.nID = bts_sendport_transform.nSendPortID
		and bts_sendport_transform.uidTransformGUID = bt_MapSpec.id
		and bt_MapSpec.assemblyid = bts_assembly.nID
		and bts_assembly.nApplicationID = @referenceAppId )
		return 0
	-- check if a send port in appId refers to a send pipeline in referenceAppId
	if exists( select 1 from bts_sendport, bts_pipeline, bts_assembly
		where bts_sendport.nApplicationID = @appId
		and bts_sendport.nSendPipelineID = bts_pipeline.Id
		and bts_pipeline.nAssemblyID = bts_assembly.nID
		and bts_assembly.nApplicationID = @referenceAppId )
		return 0
	return 1
END
