CREATE PROCEDURE [dbo].[bts_addconstraints]
AS
	-- Foreign key constraints from Admin
	alter table [dbo].[adm_ReceiveLocation] add constraint [adm_ReceiveLocation_fk_ReceivePort]
		foreign key (ReceivePortId) references [dbo].[bts_receiveport] (nID) on delete cascade
	alter table [dbo].[adm_ReceiveLocation] add constraint [adm_ReceiveLocation_fk_Pipeline]
		foreign key (ReceivePipelineId) references [dbo].[bts_pipeline] (Id)
	
	alter table [dbo].[adm_ReceiveLocation] add constraint [adm_ReceiveLocation_fk_SendPipeline]
		foreign key (SendPipelineId) references [dbo].[bts_pipeline] (Id)
	-- Foreign key constraints from PartnerMgmt
	alter table [dbo].[bts_receiveport] add constraint [bts_receiveport_foreign_sendpipelineid]
		foreign key(nSendPipelineId) references [dbo].[bts_pipeline](Id)
	alter table [dbo].[bts_sendport] add constraint [bts_sendport_foreign_sendpipelineid]
		foreign key(nSendPipelineID) references [dbo].[bts_pipeline](Id)
	alter table [dbo].[bts_sendport] add constraint [bts_sendport_foreign_receivepipelineid]
		foreign key(nReceivePipelineID) references [dbo].[bts_pipeline](Id)
	alter table [dbo].[bts_sendport_transport] add constraint [bts_sendport_transport_foreign_transporttypeid]
		foreign key(nTransportTypeId) references [dbo].[adm_Adapter](Id)
	alter table [dbo].[bts_sendport_transform] add constraint [bts_sendport_transform_foreign_transformid]
		foreign key(uidTransformGUID) references [dbo].[bt_MapSpec](id)
	alter table [dbo].[bts_receiveport_transform] add constraint [bts_receiveport_transform_foreign_transformid]
		foreign key(uidTransformGUID) references [dbo].[bt_MapSpec](id)
	alter table [dbo].[bts_orchestration_port_binding] add constraint [bts_orcport_binding_foreign_orcportid]
		foreign key(nOrcPortID) references [dbo].[bts_orchestration_port](nID) ON DELETE CASCADE
	alter table [dbo].[bts_enlistedparty] add constraint [bts_enlistedparty_foreign_roleid]
		foreign key(nRoleID) references [dbo].[bts_role](nID) ON DELETE CASCADE
		
	alter table [dbo].[bts_enlistedparty_port_mapping] add constraint [bts_enlistedparty_port_mapping_foreign_roleporttypeid]
		foreign key(nRolePortTypeID) references [dbo].[bts_role_porttype](nID)
	alter table [dbo].[bts_enlistedparty_operation_mapping] add constraint [bts_enlistedparty_operation_mapping_foreign_operationid]
		foreign key(nOperationID) references [dbo].[bts_porttype_operation](nID)
	alter table [dbo].[bts_application_reference] add constraint [bts_application_foreign_applicationid] foreign key(nApplicationID) references [dbo].[bts_application](nID)
	alter table [dbo].[bts_application_reference] add constraint bts_refapplication_foreign_applicationid	foreign key(nReferencedApplicationID) references [dbo].[bts_application](nID)
	alter table [dbo].[bts_sendport] add constraint [bts_sendport_foreign_applicationid] foreign key(nApplicationID) references [dbo].[bts_application](nID)
	alter table [dbo].[bts_sendportgroup] add constraint [bts_sendportgroup_foreign_applicationid] foreign key(nApplicationID) references [dbo].[bts_application](nID)
	
	alter table [dbo].[bts_receiveport] add constraint [bts_receiveport_foreign_applicationid] foreign key(nApplicationID) references [dbo].[bts_application](nID)
	alter table [dbo].[bts_assembly] add constraint [bts_assembly_foreign_applicationid] foreign key(nApplicationID) references [dbo].[bts_application](nID)
	ALTER TABLE [dbo].[bt_DocumentSpec] ADD CONSTRAINT [fk_bt_documentspec_bt_xmlshare] FOREIGN KEY 
		(
			[shareid]
		) REFERENCES [bt_XMLShare] (
			[id]
		)
	ALTER TABLE [dbo].[bt_DocumentSpec] ADD CONSTRAINT [fk_bt_documentspec_bts_item] FOREIGN KEY 
		(
			[itemid]
		) REFERENCES [bts_item] (
			[id]
		)
	ALTER TABLE [dbo].[bt_MapSpec] ADD CONSTRAINT [fk_bt_mapspec_bt_xmlshare] FOREIGN KEY 
		(
			[shareid]
		) REFERENCES [bt_XMLShare] (
			[id]
		)
	ALTER TABLE [dbo].[bt_MapSpec] ADD CONSTRAINT [fk_bt_mapspec_bts_item] FOREIGN KEY 
		(
			[itemid]
		) REFERENCES [bts_item] (
			[id]
		)
	ALTER TABLE [dbo].[bts_item] ADD CONSTRAINT [fk_bts_item_bts_assembly] FOREIGN KEY 
		(
			[AssemblyId]
		) REFERENCES [bts_assembly] (
			[nID]
		)
	ALTER TABLE [dbo].[bts_libreference] ADD CONSTRAINT [fk_bts_libreference_bts_assembly] FOREIGN KEY 
		(
			[idapp]
		) REFERENCES [bts_assembly] (
			[nID]
		)
	ALTER TABLE [dbo].[bts_libreference] ADD CONSTRAINT [fk_bts_libreference_bts_assembly1] FOREIGN KEY 
		(
			[idlib]
		) REFERENCES [bts_assembly] (
			[nID]
		)
	ALTER TABLE [dbo].[bts_orchestration] ADD CONSTRAINT [fk_bts_orchestration_bts_item] FOREIGN KEY 
		(
			[nItemID]
		) REFERENCES [bts_item] (
			[id]
		)
	-- Application binding check constraints
	alter table [dbo].[adm_ReceiveLocation] add constraint [CK_applicationbinding_adm_ReceiveLocation_sendpipeline]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetReceivePortAppId(ReceivePortId), dbo.adm_GetPipelineAppId(SendPipelineId)) = 1)
	alter table [dbo].[adm_ReceiveLocation] add constraint [CK_applicationbinding_adm_ReceiveLocation_receivepipeline]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetReceivePortAppId(ReceivePortId), dbo.adm_GetPipelineAppId(ReceivePipelineId)) = 1)
	alter table [dbo].[bts_orchestration_port_binding] add constraint [CK_applicationbinding_bts_orchestration_port_binding_sendportgroup]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetOrchestrationPortAppId(nOrcPortID), dbo.adm_GetSendPortGroupAppId(nSpgID)) = 1)
	alter table [dbo].[bts_orchestration_port_binding] add constraint [CK_applicationbinding_bts_orchestration_port_binding_receiveport]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetOrchestrationPortAppId(nOrcPortID), dbo.adm_GetReceivePortAppId(nReceivePortID)) = 1)
	alter table [dbo].[bts_orchestration_port_binding] add constraint [CK_applicationbinding_bts_orchestration_port_binding_sendport]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetOrchestrationPortAppId(nOrcPortID), dbo.adm_GetSendPortAppId(nSendPortID)) = 1)
	alter table [dbo].[bts_receiveport_transform] add constraint [CK_applicationbinding_bts_receiveport_transform_schema]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetReceivePortAppId(nReceivePortID), dbo.adm_GetSchemaAppId(uidTransformGUID)) = 1)
	alter table [dbo].[bts_spg_sendport] add constraint [CK_applicationbinding_bts_spg_sendport]
		check ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetSendPortGroupAppId]([nSendPortGroupID]), [dbo].[adm_GetSendPortAppId]([nSendPortID])) = 1)
	alter table [dbo].[bts_sendport_transform] add constraint [CK_applicationbinding_bts_sendport_transform_schema]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetSendPortAppId(nSendPortID), dbo.adm_GetSchemaAppId(uidTransformGUID)) = 1)
	alter table [dbo].[bts_sendport] add constraint [CK_applicationbinding_bts_sendport_sendpipeline]
		check (dbo.adm_IsReferencedBy(nApplicationID, dbo.adm_GetPipelineAppId(nSendPipelineID)) = 1)
	alter table [dbo].[bts_sendport] add constraint [CK_applicationbinding_bts_sendport_sendportgroup]
		check ([dbo].[adm_ValidateApplicationBinding_Sp]([nApplicationID], [nID]) = 1)
	alter table [dbo].[bts_sendportgroup] add constraint [CK_applicationbinding_bts_sendportgroup_sendport]
		check ([dbo].[adm_ValidateApplicationBinding_Spg]([nApplicationID], [nID]) = 1)
	alter table [dbo].[bts_application_reference] add constraint [CK_bts_application_reference_cyclic]
		check (dbo.adm_IsReferencedBy(nReferencedApplicationID, nApplicationID) = 0)
	alter table [dbo].[bts_enlistedparty_operation_mapping] add constraint [CK_applicationbinding_bts_enlistedparty_operation_mapping]
		check (dbo.adm_IsReferencedBy(dbo.adm_GetRolelinkTypeNonSystemAppId(nPortMappingID), dbo.adm_GetPartySendPortAppId(nPartySendPortID)) = 1)
