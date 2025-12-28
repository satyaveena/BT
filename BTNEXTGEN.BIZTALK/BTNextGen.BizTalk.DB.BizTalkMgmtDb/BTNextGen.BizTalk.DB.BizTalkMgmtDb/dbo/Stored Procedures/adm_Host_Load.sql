CREATE PROCEDURE [dbo].[adm_Host_Load]
@Name nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 -- Load rest of properties of the host
 select 
  adm_Host.Id, 
  adm_Host.Name, 
  adm_Host.NTGroupName, 
  adm_Host.LastUsedLogon,
  case (select count(*) from adm_Group where DefaultHostId = adm_Host.Id)
   when 0 then 0 else -1 end as DefaultHost,
  adm_Host.HostTracking,
  adm_Host.AuthTrusted,
  adm_Host.HostType,
  adm_Host.DateModified,
  adm_Host.DecryptCertName,
  adm_Host.DecryptCertThumbprint,
  dbo.adm_GetNumInstalledHostInstances(adm_Host.Name),
  adm_Host.ClusterResourceGroupName,
  adm_Host.IsHost32BitOnly,
  adm_Host.MessageDeliverySampleSpaceSize,
  adm_Host.MessageDeliverySampleSpaceWindow,
  adm_Host.MessageDeliveryOverdriveFactor,
  adm_Host.MessageDeliveryMaximumDelay,
  adm_Host.MessagePublishSampleSpaceSize,
  adm_Host.MessagePublishSampleSpaceWindow,
  adm_Host.MessagePublishOverdriveFactor,
  adm_Host.MessagePublishMaximumDelay,
  adm_Host.DeliveryQueueSize,
  adm_Host.DBSessionThreshold,
  adm_Host.GlobalMemoryThreshold,
  adm_Host.ProcessMemoryThreshold,
  adm_Host.ThreadThreshold,
  adm_Host.DBQueueSizeThreshold,
  adm_Host.InflightMessageThreshold,
  adm_Host.ThreadPoolSize,
  UseDefaultAppDomainForIsolatedAdapter,
  AllowMultipleResponses,
  MessagingMaxReceiveInterval,
  XlangMaxReceiveInterval,
  MessagingReqRespTTL,
  MsgAgentPerfCounterServiceClassID,
  LegacyWhitespace,
  ThrottlingSpoolMultiplier,
  ThrottlingTrackingDataMultiplier,
  ThrottlingLimitToTriggerGC,
  ThrottlingBatchMemoryThresholdPercent,
  ThrottlingSeverityProcessMemory,
  ThrottlingSeverityDatabaseSize,
  ThrottlingSeverityInflightMessage,
  ThrottlingPublishOverrideSeverity,
  ThrottlingDeliveryOverrideSeverity,
  ThrottlingPublishOverride,
  ThrottlingDeliveryOverride,
  DehydrationBehavior,
  TimeBasedMaxThreshold,
  TimeBasedMinThreshold,
  SubscriptionPauseAt,
  SubscriptionResumeAt
 from
  adm_Host,
     (select HostId,           
           MAX(case when PropertyName= N'UseDefaultAppDomainForIsolatedAdapter' then CAST(PropertyValue as int) END) as UseDefaultAppDomainForIsolatedAdapter,
           MAX(case when PropertyName= N'AllowMultipleResponses' then CAST(PropertyValue as int) END) as AllowMultipleResponses,
           MAX(case when PropertyName= N'MessagingMaxReceiveInterval' then CAST(PropertyValue as int) END) as MessagingMaxReceiveInterval,
           MAX(case when PropertyName= N'XlangMaxReceiveInterval' then CAST(PropertyValue as int) END) as XlangMaxReceiveInterval,
           MAX(case when PropertyName= N'MessagingReqRespTTL' then CAST(PropertyValue as int) END) as MessagingReqRespTTL,
           MAX(case when PropertyName= N'MsgAgentPerfCounterServiceClassID' then CAST(PropertyValue as int) END) as MsgAgentPerfCounterServiceClassID,
           MAX(case when PropertyName= N'LegacyWhitespace' then CAST(PropertyValue as int) END) AS LegacyWhitespace,
           MAX(case when PropertyName= N'ThrottlingSpoolMultiplier' then CAST(PropertyValue AS int) END) as ThrottlingSpoolMultiplier,
           MAX(case when PropertyName= N'ThrottlingTrackingDataMultiplier' then CAST(PropertyValue as int) END) as ThrottlingTrackingDataMultiplier,
           MAX(case when PropertyName= N'ThrottlingLimitToTriggerGC' then CAST(PropertyValue as int) END) as ThrottlingLimitToTriggerGC,
           MAX(case when PropertyName= N'ThrottlingBatchMemoryThresholdPercent' then CAST(PropertyValue as int) END) as ThrottlingBatchMemoryThresholdPercent,
           MAX(case when PropertyName= N'ThrottlingSeverityProcessMemory' then CAST(PropertyValue as int) END) as ThrottlingSeverityProcessMemory,
           MAX(case when PropertyName= N'ThrottlingSeverityDatabaseSize' then CAST(PropertyValue as int) END) as ThrottlingSeverityDatabaseSize,
           MAX(case when PropertyName= N'ThrottlingSeverityInflightMessage' then CAST(PropertyValue as int) END) as ThrottlingSeverityInflightMessage,
           MAX(case when PropertyName= N'ThrottlingPublishOverrideSeverity' then CAST(PropertyValue as int) END) as ThrottlingPublishOverrideSeverity,
           MAX(case when PropertyName= N'ThrottlingDeliveryOverrideSeverity' then CAST(PropertyValue as int) END) as ThrottlingDeliveryOverrideSeverity,
           MAX(case when PropertyName= N'ThrottlingPublishOverride' then CAST(PropertyValue as int) END) as ThrottlingPublishOverride,
           MAX(case when PropertyName= N'ThrottlingDeliveryOverride' then CAST(PropertyValue as int) END) as ThrottlingDeliveryOverride,
           MAX(case when PropertyName= N'DehydrationBehavior' then CAST(PropertyValue as int) END) as DehydrationBehavior,
           MAX(case when PropertyName= N'TimeBasedMaxThreshold' then CAST(PropertyValue as int) END) as TimeBasedMaxThreshold,
           MAX(case when PropertyName= N'TimeBasedMinThreshold' then CAST(PropertyValue as int) END) as TimeBasedMinThreshold,
           MAX(case when PropertyName= N'SubscriptionPauseAt' then CAST(PropertyValue as int) END) as SubscriptionPauseAt,
           MAX(case when PropertyName= N'SubscriptionResumeAt' then CAST(PropertyValue as int) END) as SubscriptionResumeAt
         From adm_HostSetting
         Group BY HostId) temp
 where
  adm_Host.Name = @Name and Id = temp.HostId
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Load] TO [BTS_OPERATORS]
    AS [dbo];

