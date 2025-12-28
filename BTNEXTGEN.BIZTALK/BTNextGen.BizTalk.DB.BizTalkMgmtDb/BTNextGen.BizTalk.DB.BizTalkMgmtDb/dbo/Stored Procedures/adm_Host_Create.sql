CREATE PROCEDURE [dbo].[adm_Host_Create]
@Name nvarchar(80),
@NTGroupName nvarchar(128),
@LastUsedLogon nvarchar(128),
@DefaultHost int,
@HostTracking int,
@AuthTrusted int,
@HostType int,
@DecryptCertName nvarchar(256),
@DecryptCertThumbprint nvarchar(80),
@ClusterResourceGroupName nvarchar(256),
@IsHost32BitOnly bit,
@MessageDeliverySampleSpaceSize int,
@MessageDeliverySampleSpaceWindow int,
@MessageDeliveryOverdriveFactor int,
@MessageDeliveryMaximumDelay int,
@MessagePublishSampleSpaceSize int,
@MessagePublishSampleSpaceWindow int,
@MessagePublishOverdriveFactor int,
@MessagePublishMaximumDelay int,
@DeliveryQueueSize int,
@DBSessionThreshold int,
@GlobalMemoryThreshold int,
@ProcessMemoryThreshold int,
@ThreadThreshold int,
@DBQueueSizeThreshold int,
@InflightMessageThreshold int,
@ThreadPoolSize int,
@UseDefaultAppDomainForIsolatedAdapter int,
@AllowMultipleResponses int,
@MessagingMaxReceiveInterval int,
@XlangMaxReceiveInterval int,
@MessagingReqRespTTL int,
@MsgAgentPerfCounterServiceClassID int,
@LegacyWhitespace int,
@ThrottlingSpoolMultiplier int,
@ThrottlingTrackingDataMultiplier int,
@ThrottlingLimitToTriggerGC int,
@ThrottlingBatchMemoryThresholdPercent int,
@ThrottlingSeverityProcessMemory int,
@ThrottlingSeverityDatabaseSize int,
@ThrottlingSeverityInflightMessage int,
@ThrottlingPublishOverrideSeverity int,
@ThrottlingDeliveryOverrideSeverity int,
@ThrottlingPublishOverride int,
@ThrottlingDeliveryOverride int,
@DehydrationBehavior int,
@TimeBasedMaxThreshold int,
@TimeBasedMinThreshold int,
@SubscriptionPauseAt int,
@SubscriptionResumeAt int
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int, @bHasOpenedTransaction as int
 select @ErrCode = 0, @bHasOpenedTransaction=0
 
 if (@SubscriptionResumeAt <> 0 and @SubscriptionPauseAt <= @SubscriptionResumeAt)
 begin
     Set @ErrCode = 0xC0C02601  -- CIS_E_ADMIN_CORE_SUBSCRIPTION_PAUSEAT_LESS_OR_EQUAL_SUBSCRIPTION_PAUSEAT
     return @ErrCode
 end
 
 if (@TimeBasedMaxThreshold <= @TimeBasedMinThreshold)
 begin
     Set @ErrCode = 0xC0C02604  -- CIS_E_ADMIN_CORE_TIMEBASED_MAX_THRESHOLD_LESS_OR_EQUALTO_TIMEBASED_MIN_THRESHOLD
     return @ErrCode
 end

    -- This is special case because we have to do both min and max check
 if (@ThrottlingLimitToTriggerGC < 50 )
 begin
     Set @ErrCode = 0xC0C02605  -- CIS_E_ADMIN_CORE_THROTTLING_LIMIT_TO_TRIGGERGC_OUT_OF_RANGE
     return @ErrCode
 end
 if(@@trancount = 0) -- this sp could be called from DTC
 begin
  begin transaction
  set @bHasOpenedTransaction=1
 end
  declare @BIZTALK_UNKNOWN_DBID int
  set @BIZTALK_UNKNOWN_DBID = 0
  -- Create new adm_Host record
  insert into adm_Host 
   ( GroupId, Name, NTGroupName, LastUsedLogon, HostTracking, AuthTrusted, HostType, DecryptCertName, DecryptCertThumbprint,
    ClusterResourceGroupName,
    IsHost32BitOnly,
    MessageDeliverySampleSpaceSize,
    MessageDeliverySampleSpaceWindow,
    MessageDeliveryOverdriveFactor,
    MessageDeliveryMaximumDelay,
    MessagePublishSampleSpaceSize,
    MessagePublishSampleSpaceWindow,
    MessagePublishOverdriveFactor,
    MessagePublishMaximumDelay,
    DeliveryQueueSize,
    DBSessionThreshold,
    GlobalMemoryThreshold,
    ProcessMemoryThreshold,
    ThreadThreshold,
    DBQueueSizeThreshold,
    InflightMessageThreshold,
    ThreadPoolSize)
  values
   ( dbo.adm_GetGroupId(), @Name, @NTGroupName, @LastUsedLogon, @HostTracking, @AuthTrusted, @HostType, @DecryptCertName, @DecryptCertThumbprint,
    @ClusterResourceGroupName,
    @IsHost32BitOnly,
    @MessageDeliverySampleSpaceSize,
    @MessageDeliverySampleSpaceWindow,
    @MessageDeliveryOverdriveFactor,
    @MessageDeliveryMaximumDelay,
    @MessagePublishSampleSpaceSize,
    @MessagePublishSampleSpaceWindow,
    @MessagePublishOverdriveFactor,
    @MessagePublishMaximumDelay,
    @DeliveryQueueSize,
    @DBSessionThreshold,
    @GlobalMemoryThreshold,
    @ProcessMemoryThreshold,
    @ThreadThreshold,
    @DBQueueSizeThreshold,
    @InflightMessageThreshold,
    @ThreadPoolSize
   )
  declare @HostId as int
  set @HostId = @@Identity
  -- If this is Isolated host, make sure it cannot host tracking and cannot be default host
  if ( @HostType = 2 AND (@DefaultHost <> 0 OR @HostTracking <> 0) )
  begin
   set @ErrCode = 0xC0C025C3 -- CIS_E_ADMIN_ISOLATED_HOST_CONFLICT
   goto exit_proc
  end
  
  -- Write the host_setting values
  insert into adm_HostSetting (HostId,PropertyName, PropertyValue)
  select @HostId, N'UseDefaultAppDomainForIsolatedAdapter', CAST(@UseDefaultAppDomainForIsolatedAdapter as nvarchar)
  union all
  select @HostId, N'AllowMultipleResponses', CAST(@AllowMultipleResponses as nvarchar)
  union all
  select @HostId, N'MessagingMaxReceiveInterval', CAST(@MessagingMaxReceiveInterval as nvarchar)
  union all
  select @HostId, N'XlangMaxReceiveInterval', CAST(@XlangMaxReceiveInterval as nvarchar)
  union all
  select @HostId, N'MessagingReqRespTTL', CAST(@MessagingReqRespTTL as nvarchar)
  union all
  select @HostId, N'MsgAgentPerfCounterServiceClassID', CAST(@MsgAgentPerfCounterServiceClassID as nvarchar)
  union all
  select @HostId, N'LegacyWhitespace', CAST(@LegacyWhitespace as nvarchar)
  union all
  select @HostId, N'ThrottlingSpoolMultiplier', CAST(@ThrottlingSpoolMultiplier as nvarchar)
  union all
  select @HostId, N'ThrottlingTrackingDataMultiplier', CAST(@ThrottlingTrackingDataMultiplier as nvarchar)
  union all
  select @HostId, N'ThrottlingLimitToTriggerGC', CAST(@ThrottlingLimitToTriggerGC as nvarchar)
  union all
  select @HostId, N'ThrottlingBatchMemoryThresholdPercent', CAST(@ThrottlingBatchMemoryThresholdPercent as nvarchar)
  union all
  select @HostId, N'ThrottlingSeverityProcessMemory', CAST(@ThrottlingSeverityProcessMemory as nvarchar)
  union all
  select @HostId, N'ThrottlingSeverityDatabaseSize', CAST(@ThrottlingSeverityDatabaseSize as nvarchar)
  union all
  select @HostId, N'ThrottlingSeverityInflightMessage', CAST(@ThrottlingSeverityInflightMessage as nvarchar)
  union all
  select @HostId, N'ThrottlingPublishOverrideSeverity', CAST(@ThrottlingPublishOverrideSeverity as nvarchar)
  union all
  select @HostId, N'ThrottlingDeliveryOverrideSeverity', CAST(@ThrottlingDeliveryOverrideSeverity as nvarchar)
  union all
  select @HostId, N'ThrottlingPublishOverride', CAST(@ThrottlingPublishOverride as nvarchar)
  union all
  select @HostId, N'ThrottlingDeliveryOverride', CAST(@ThrottlingDeliveryOverride as nvarchar)
  union all
  select @HostId, N'DehydrationBehavior', CAST(@DehydrationBehavior as nvarchar)
  union all
  select @HostId, N'TimeBasedMaxThreshold', CAST(@TimeBasedMaxThreshold as nvarchar)
  union all
  select @HostId, N'TimeBasedMinThreshold', CAST(@TimeBasedMinThreshold as nvarchar)
  union all
  select @HostId, N'SubscriptionPauseAt', CAST(@SubscriptionPauseAt as nvarchar)
  union all
  select @HostId, N'SubscriptionResumeAt', CAST(@SubscriptionResumeAt as nvarchar)

  -- Is this the first host?
  if not exists ( select * from adm_Host where Id <> @HostId )
  begin
  
   -- First host must be default with In-Process host type and it must host tracking
   if ( 0 = @DefaultHost OR 0 = @HostTracking OR 1 <> @HostType ) -- eHostTypeInProcess
   begin
     set @ErrCode = 0xC0C02559 -- CIS_E_ADMIN_FIRST_APP_TYPE_MUST_BE_DEFAULT
     goto exit_proc
   end
  end
  
  -- Reassign default host when "DefaultHost=true"
  if (0 <> @DefaultHost)
  begin
   update adm_Group set DefaultHostId = @HostId
  end
  -- Create the necessary adm_Server2HostMapping records
  insert into adm_Server2HostMapping
   (ServerId, HostId, IsMapped)
  select
   adm_Server.Id,
   @HostId,
   0  -- by default new Host is never Mapped
  from
   adm_Server
exit_proc:
 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
  begin
   rollback transaction
  end
 end
 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

