CREATE PROCEDURE [dbo].[bts_removeconstraints]
AS
	-- Foreign key constraints from Admin
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocation_fk_ReceivePort]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[adm_ReceiveLocation] drop constraint adm_ReceiveLocation_fk_ReceivePort
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocation_fk_Pipeline]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[adm_ReceiveLocation] drop constraint adm_ReceiveLocation_fk_Pipeline
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocation_fk_SendPipeline]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[adm_ReceiveLocation] drop constraint adm_ReceiveLocation_fk_SendPipeline
	------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Foreign key constraints from Deployment
	--**********************************************************************************************************************
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bt_documentspec_bt_xmlshare]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bt_DocumentSpec] DROP CONSTRAINT [fk_bt_documentspec_bt_xmlshare]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bt_documentspec_bts_item]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bt_DocumentSpec] DROP CONSTRAINT [fk_bt_documentspec_bts_item]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bt_mapspec_bt_xmlshare]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bt_MapSpec] DROP CONSTRAINT [fk_bt_mapspec_bt_xmlshare]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bt_mapspec_bts_item]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bt_MapSpec] DROP CONSTRAINT [fk_bt_mapspec_bts_item]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bts_item_bts_assembly]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bts_item] DROP CONSTRAINT [fk_bts_item_bts_assembly]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bts_libreference_bts_assembly]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bts_libreference] DROP CONSTRAINT [fk_bts_libreference_bts_assembly]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bts_libreference_bts_assembly1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bts_libreference] DROP CONSTRAINT [fk_bts_libreference_bts_assembly1]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fk_bts_orchestration_bts_item]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
	ALTER TABLE [dbo].[bts_orchestration] DROP CONSTRAINT [fk_bts_orchestration_bts_item]
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Foreign key constraints from PartnerMgmt
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_receiveport_foreign_sendpipelineid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_receiveport] drop constraint [bts_receiveport_foreign_sendpipelineid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_foreign_sendpipelineid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport] drop constraint [bts_sendport_foreign_sendpipelineid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_foreign_applicationtypeid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport] drop constraint [bts_sendport_foreign_applicationtypeid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_foreign_receivepipelineid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport] drop constraint [bts_sendport_foreign_receivepipelineid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_transport_foreign_transporttypeid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport_transport] drop constraint [bts_sendport_transport_foreign_transporttypeid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_transform_foreign_transformid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport_transform] drop constraint [bts_sendport_transform_foreign_transformid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_receiveport_transform_foreign_transformid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_receiveport_transform] drop constraint [bts_receiveport_transform_foreign_transformid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_orcport_binding_foreign_orcportid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_orchestration_port_binding] drop constraint [bts_orcport_binding_foreign_orcportid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_enlistedparty_foreign_roleid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_enlistedparty] drop constraint [bts_enlistedparty_foreign_roleid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_enlistedparty_port_mapping_foreign_roleporttypeid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_enlistedparty_port_mapping] drop constraint [bts_enlistedparty_port_mapping_foreign_roleporttypeid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_enlistedparty_operation_mapping_foreign_operationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_enlistedparty_operation_mapping] drop constraint [bts_enlistedparty_operation_mapping_foreign_operationid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_application_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_application_reference] drop constraint [bts_application_foreign_applicationid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_refapplication_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_application_reference] drop constraint [bts_refapplication_foreign_applicationid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendport_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendport] drop constraint [bts_sendport_foreign_applicationid]
		
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_sendportgroup_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_sendportgroup] drop constraint [bts_sendportgroup_foreign_applicationid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_receiveport_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_receiveport] drop constraint [bts_receiveport_foreign_applicationid]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bts_assembly_foreign_applicationid]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[bts_assembly] drop constraint [bts_assembly_foreign_applicationid]
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Application binding check constraints
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_adm_ReceiveLocation_sendpipeline]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[adm_ReceiveLocation] drop constraint [CK_applicationbinding_adm_ReceiveLocation_sendpipeline]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_adm_ReceiveLocation_receivepipeline]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[adm_ReceiveLocation] drop constraint [CK_applicationbinding_adm_ReceiveLocation_receivepipeline]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_orchestration_port_binding_sendportgroup]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_orchestration_port_binding] drop constraint [CK_applicationbinding_bts_orchestration_port_binding_sendportgroup]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_orchestration_port_binding_receiveport]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_orchestration_port_binding] drop constraint [CK_applicationbinding_bts_orchestration_port_binding_receiveport]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_orchestration_port_binding_sendport]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_orchestration_port_binding] drop constraint [CK_applicationbinding_bts_orchestration_port_binding_sendport]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_receiveport_transform_schema]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_receiveport_transform] drop constraint [CK_applicationbinding_bts_receiveport_transform_schema]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_spg_sendport]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_spg_sendport] drop constraint [CK_applicationbinding_bts_spg_sendport]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_sendport_transform_schema]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_sendport_transform] drop constraint [CK_applicationbinding_bts_sendport_transform_schema]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_sendport_sendpipeline]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_sendport] drop constraint [CK_applicationbinding_bts_sendport_sendpipeline]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_sendport_sendportgroup]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_sendport] drop constraint [CK_applicationbinding_bts_sendport_sendportgroup]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_sendportgroup_sendport]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_sendportgroup] drop constraint [CK_applicationbinding_bts_sendportgroup_sendport]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_bts_application_reference_cyclic]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_application_reference] drop constraint [CK_bts_application_reference_cyclic]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CK_applicationbinding_bts_enlistedparty_operation_mapping]') and OBJECTPROPERTY(id, N'IsCheckCnst') = 1)
		alter table [dbo].[bts_enlistedparty_operation_mapping] drop constraint [CK_applicationbinding_bts_enlistedparty_operation_mapping]
