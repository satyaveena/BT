CREATE PROCEDURE [dbo].[adm_removeschema]
AS
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_removeschema]') and OBJECTPROPERTY(id, N'IsProcedure') = 1) drop procedure [dbo].[adm_removeschema]
   --/---------------------------------------------------------------------------------------------------------------
   --// Trackinginterceptor Configuration tables
   --/---------------------------------------------------------------------------------------------------------------
   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[StaticTrackingInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[StaticTrackingInfo]
   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TrackinginterceptorVersions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[TrackinginterceptorVersions]
   if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Trackinginterceptor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[Trackinginterceptor]
   
	--/---------------------------------------------------------------------------------------------------------------
	--// Specific Constraints
	--/---------------------------------------------------------------------------------------------------------------
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group_fk_DefaultHost]')and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
		alter table [dbo].[adm_Group] drop constraint adm_Group_fk_DefaultHost
	--/---------------------------------------------------------------------------------------------------------------
	--// Admin tables
	--/---------------------------------------------------------------------------------------------------------------
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ServiceClass]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_ServiceClass]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveLocation]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_ReceiveLocation]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_AdapterAlias]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_AdapterAlias]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_DefaultReceiveHandler]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_DefaultReceiveHandler]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveHandler]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_ReceiveHandler]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_SendHandler]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_SendHandler]
	
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_ReceiveService]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_ReceiveService]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstanceZombie]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_HostInstanceZombie]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_HostInstance]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstanceSetting]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_HostInstanceSetting]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server2HostMapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_Server2HostMapping]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_Host]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostSetting]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_HostSetting]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_HostInstance_SubServices]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_HostInstance_SubServices]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_MessageBox]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_MessageBox]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_Group]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_GroupSetting]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_GroupSetting]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Server]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_Server]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_Adapter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[adm_Adapter]
	--/---------------------------------------------------------------------------------------------------------------
	--// Backup tables and views
	--/---------------------------------------------------------------------------------------------------------------
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_BackupHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) 
		DROP TABLE [dbo].[adm_BackupHistory]
	if exists (select * from sysobjects where id = object_id(N'[dbo].[adm_OtherBackupDatabases]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 		DROP TABLE [dbo].[adm_OtherBackupDatabases]
		
	IF exists (select * from sysobjects where id = object_id(N'[dbo].[adm_BackupSettings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[adm_BackupSettings]
	IF exists (select * from sysobjects where id = object_id(N'[dbo].[adm_BackupSetId]' ) and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[adm_BackupSetId]
	
	IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[admv_BackupDatabases]') and OBJECTPROPERTY(id, N'IsView') = 1)
		DROP VIEW [dbo].[admv_BackupDatabases]
	--/------------------------------------------------------------------------------------------------------
	--// Views for getting local and utc date values for access in user functions
	--/------------------------------------------------------------------------------------------------------
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[admv_UTCDate]') and OBJECTPROPERTY(id, N'IsView') = 1) drop view [dbo].[admv_UTCDate]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[admv_LocalDate]') and OBJECTPROPERTY(id, N'IsView') = 1) drop view [dbo].[admv_LocalDate]
	--/------------------------------------------------------------------------------------------------------
	--// BAS Table
	--/------------------------------------------------------------------------------------------------------
	IF exists (select * from sysobjects where id = object_id(N'[dbo].[bas_Properties]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
		DROP TABLE [dbo].[bas_Properties]
	--/------------------------------------------------------------------------------------------------------
	--// TPE Table
	--/------------------------------------------------------------------------------------------------------
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bam_TrackPoints]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bam_TrackPoints]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bam_TrackingProfiles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bam_TrackingProfiles]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bam_ActivityToOrchestrationMapping]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[bam_ActivityToOrchestrationMapping]
	
	--Monitor BizTalk server SQL job realted tables
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[btsmon_Inconsistancies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[btsmon_Inconsistancies]
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[btsmon_Issues]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[btsmon_Issues]
