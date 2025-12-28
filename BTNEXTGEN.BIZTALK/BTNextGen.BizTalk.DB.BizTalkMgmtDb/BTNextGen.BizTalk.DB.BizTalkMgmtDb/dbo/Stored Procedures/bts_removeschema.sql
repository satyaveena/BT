CREATE PROCEDURE [dbo].[bts_removeschema]
AS
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_removeschema]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[bts_removeschema]
	
	--/---------------------------------------------------------------------------------------------------------------
	--// Partner Management tables
	--/---------------------------------------------------------------------------------------------------------------
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_enlistedparty_operation_mapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_enlistedparty_operation_mapping]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_enlistedparty_port_mapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_enlistedparty_port_mapping]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_enlistedparty]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_enlistedparty]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_orchestration_port_binding]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_orchestration_port_binding]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_receiveport_transform]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_receiveport_transform]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_receiveport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_receiveport]
	
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_spg_sendport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_spg_sendport]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_sendportgroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_sendportgroup]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_dynamicport_subids]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_dynamicport_subids]
	
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_party_alias]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_party_alias]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_party_sendport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_party_sendport]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_party]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_party]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_sendport_transform]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_sendport_transform]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_sendport_transport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_sendport_transport]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_sendport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_sendport]
	-- Tables to support the application feature
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_application_reference]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_application_reference]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[bts_application]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bts_application]
