CREATE PROCEDURE [dbo].[adm_CleanupMgmtDB]
AS
 exec sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
 DELETE FROM adm_Group WHERE Name <> N'Biztalk Group'
 DELETE FROM adm_Host WHERE Name NOT IN ( N'BizTalkServerApplication', N'BizTalkServerIsolatedHost' )
 --DELETE FROM adm_DefaultHost WHERE GroupId NOT IN (SELECT Id FROM adm_Group) 
 DELETE FROM adm_MessageBox WHERE GroupId NOT IN (SELECT Id FROM adm_Group)

 --I will assume that this is the first row and only leave that server in the db
 DELETE FROM adm_Server WHERE Id <> 1
 DELETE FROM adm_Server2HostMapping WHERE ServerId <> 1
 DELETE FROM adm_HostInstance WHERE Svr2HostMappingId NOT IN (SELECT Id FROM adm_Server2HostMapping)

 --We'll just leave these tables alone since there is no way to add rows to them
 --truncate table adm_HostInstance_SubServices
 --truncate table adm_ServiceClass

 --Keep only MSMQT, HTTP, FILE, SMTP, SOAP and SQL adapter related rows
 DELETE FROM adm_Adapter WHERE Name NOT IN (N'MSMQT', N'HTTP', N'SMTP', N'FILE', N'SOAP', N'SQL', N'FTP', N'MSMQ', N'POP3', N'MQS' )
 DELETE FROM adm_SendHandler WHERE AdapterId NOT IN (SELECT Id FROM adm_Adapter) OR
      GroupId NOT IN (SELECT Id FROM adm_Group)
 DELETE FROM adm_ReceiveHandler WHERE AdapterId NOT IN (SELECT Id FROM adm_Adapter) OR
      GroupId NOT IN (SELECT Id FROM adm_Group)


 --Lets remove all modules except for the DefaultPipelines and the GlobalPropertySchemas
 DELETE FROM bts_assembly WHERE nvcName NOT IN (N'Microsoft.BizTalk.GlobalPropertySchemas', N'Microsoft.BizTalk.DefaultPipelines')
 DELETE FROM bt_DocumentSpec WHERE assemblyid NOT IN (SELECT nID FROM bts_assembly)
 DELETE FROM bts_item WHERE AssemblyId NOT IN (SELECT nID FROM bts_assembly)
 DELETE FROM bt_XMLShare WHERE id NOT IN (SELECT DISTINCT(shareid) FROM bt_DocumentSpec)

 --delete all but the default pipelines
 DELETE FROM bts_pipeline WHERE PipelineID NOT IN (SELECT Guid FROM bts_item)
 DELETE FROM bts_pipeline_config WHERE PipelineID NOT IN (SELECT Id FROM bts_pipeline)
 DELETE FROM bts_pipeline_stage WHERE Id NOT IN (SELECT StageID FROM bts_pipeline_config)
 DELETE FROM bts_stage_config WHERE StageID NOT IN (SELECT StageID FROM bts_pipeline_config)
 DELETE FROM bts_component WHERE Id NOT IN (SELECT CompID FROM bts_stage_config)

 --There is a view which references these tables so I can't truncate them
 DELETE FROM bts_orchestration
 DELETE FROM bts_assembly_orchestration_Mapping

 DELETE FROM bts_messagetype_part
 DELETE FROM bts_messagetype
 DELETE FROM bts_operation_msgtype
 DELETE FROM bts_assembly_msgtype_Mapping
 DELETE FROM bts_porttype_operation
 DELETE FROM bts_porttype
 DELETE FROM bts_assembly_porttype_Mapping
 DELETE FROM bts_role_porttype
 DELETE FROM bts_role
 DELETE FROM bts_rolelink_type
 DELETE FROM bts_assembly_rolelink_type_Mapping
 DELETE FROM bts_orchestration_port
 --DELETE FROM bts_transform_service


 DELETE FROM StaticTrackingInfo
 DELETE FROM adm_HostInstanceZombie
 DELETE FROM adm_ReceiveLocation
 DELETE FROM bt_MapSpec
 DELETE FROM bt_Properties
 DELETE FROM bts_itemreference
 DELETE FROM bts_party_alias
 DELETE FROM bts_enlistedparty_operation_Mapping
 DELETE FROM bts_enlistedparty_port_Mapping
 DELETE FROM bts_enlistedparty
 DELETE FROM bts_party
 DELETE FROM bts_party_sendport
 DELETE FROM bts_spg_sendport
 DELETE FROM bts_libreference
 DELETE FROM bts_sendport_transport
 DELETE FROM bts_orchestration_invocation
 DELETE FROM bts_rolelink
 DELETE FROM bts_orchestration_port_binding
 --DELETE FROM bts_transformservice_msg
 DELETE FROM bts_receiveport
 DELETE FROM adm_receivelocation
 DELETE FROM bts_sendport
 DELETE FROM bts_sendportgroup

 -- Clean the BAS related table
 DELETE FROM bas_Properties
 exec sp_MSforeachtable "ALTER TABLE ? CHECK CONSTRAINT all"