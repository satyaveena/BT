CREATE PROCEDURE [dbo].[adm_Host_Update]
@Name nvarchar(80),
@NTGroupName nvarchar(128), -- this property is not updatable.
@LastUsedLogon nvarchar(128),
@DefaultHost int,
@HostTracking int,
@AuthTrusted int,
@HostType int, -- this property is readonly post-creation, so it is ignored
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
 declare @ErrCode as int, @HostId as int, @OrigHostType as int, @bHasOpenedTransaction as int
 select @ErrCode = 0, @HostId = 0, @OrigHostType = 0, @bHasOpenedTransaction = 0
 
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
  -- Resolve foreign key: GroupId, HostId
  select @HostId = Id, @OrigHostType = HostType
  from adm_Host
  where Name = @Name
  
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  --prefetch old HostTracking Value before updating table with the new one (for TDDS)
  declare @UsedToHostTracking as int
  select @UsedToHostTracking = HostTracking from adm_Host where Id = @HostId
  -- verify that "NTGroupName" property is not changed if there is already at least one host instance installed
  if exists ( select * from adm_Host
     where Id = @HostId  AND NTGroupName <> @NTGroupName)
  begin
   declare @NumHostInstances as int
   select @NumHostInstances = count(*)
   from adm_Server2HostMapping mapping, adm_HostInstance inst
   where
    @HostId = mapping.HostId
    AND mapping.Id = inst.Svr2HostMappingId
   if ( @NumHostInstances > 0 )
   begin
    set @ErrCode = 0xC0C02556 -- CIS_E_ADMIN_CORE_NT_GRP_NAME_CANNOT_CHANGE
    goto exit_proc
   end
  end
  -- Ensure that "DefaultHost" property cannot switch off, only swich on.
  if exists ( select * from adm_Group
     where DefaultHostId = @HostId AND 0 = @DefaultHost)
  begin
   set @ErrCode = 0xC0C02555 -- CIS_E_ADMIN_CORE_DEF_APP_CANNOT_CHANGE
   goto exit_proc
  end
  
  -- Is this LoginName being used by other Host with a DIFFERENT AuthTrusted setting?
  -- If so, then this violates our single trusted domain constraint and we need to
  -- throw an error.
  select distinct HostInst.LoginName
  into #LoginNamesToCheck
  from 
   adm_Server2HostMapping Mapping,
   adm_HostInstance HostInst
  where
   @HostId = Mapping.HostId
   AND Mapping.Id = HostInst.Svr2HostMappingId
  if exists (
   select *
   from
    adm_Host Host,
    adm_Server2HostMapping Mapping,
    adm_HostInstance HostInst
   where
    Host.Id <> @HostId
    AND Host.AuthTrusted <> @AuthTrusted
    AND Host.Id = Mapping.HostId
    AND Mapping.Id = HostInst.Svr2HostMappingId
    AND HostInst.LoginName in (select * from #LoginNamesToCheck)
  )
  begin
   set @ErrCode = 0xC0C025BE -- CIS_E_ADMIN_AUTH_TRUSTED_LOGIN_CONFLICT
   goto exit_proc
  end
  
  -- Ensure that "HostType" property cannot be changed (TO DO: need to add new "WriteOnCreate" constraint into WMI framework)
  --if ( @HostType <> @OrigHostType )
  --begin
  -- set @ErrCode = 0xC0C025C4 -- CIS_E_ADMIN_CORE_HOST_TYPE_CANNOT_CHANGE
  -- goto exit_proc
  --end
  
  -- If this is Isolated host, make sure it cannot host tracking and cannot be default host
  if ( @OrigHostType = 2 AND (@DefaultHost <> 0 OR @HostTracking <> 0) )
  begin
   set @ErrCode = 0xC0C025C3 -- CIS_E_ADMIN_ISOLATED_HOST_CONFLICT
   goto exit_proc
  end
  
  -- Update adm_HostSetting
        update adm_HostSetting SET PropertyValue = 
  (CASE
           when PropertyName = N'UseDefaultAppDomainForIsolatedAdapter' then CAST(@UseDefaultAppDomainForIsolatedAdapter as nvarchar)
           when PropertyName = N'AllowMultipleResponses' then CAST(@AllowMultipleResponses as nvarchar) 
           when PropertyName = N'MessagingMaxReceiveInterval' then CAST(@MessagingMaxReceiveInterval as nvarchar)
           when PropertyName = N'XlangMaxReceiveInterval' then CAST(@XlangMaxReceiveInterval as nvarchar)          
           when PropertyName = N'MessagingReqRespTTL' then CAST(@MessagingReqRespTTL as nvarchar)
           when PropertyName = N'MsgAgentPerfCounterServiceClassID' then CAST(@MsgAgentPerfCounterServiceClassID as nvarchar)
           when PropertyName = N'LegacyWhitespace' then CAST(@LegacyWhitespace as nvarchar)
           when PropertyName = N'ThrottlingSpoolMultiplier' then CAST(@ThrottlingSpoolMultiplier as nvarchar) 
           when PropertyName = N'ThrottlingTrackingDataMultiplier' then CAST(@ThrottlingTrackingDataMultiplier as nvarchar)
           when PropertyName = N'ThrottlingLimitToTriggerGC' then CAST(@ThrottlingLimitToTriggerGC as nvarchar) 
           when PropertyName = N'ThrottlingBatchMemoryThresholdPercent' then CAST(@ThrottlingBatchMemoryThresholdPercent as nvarchar)
           when PropertyName = N'ThrottlingSeverityProcessMemory' then CAST(@ThrottlingSeverityProcessMemory as nvarchar)
           when PropertyName = N'ThrottlingSeverityDatabaseSize' then CAST(@ThrottlingSeverityDatabaseSize as nvarchar)
           when PropertyName = N'ThrottlingSeverityInflightMessage' then CAST(@ThrottlingSeverityInflightMessage as nvarchar)
           when PropertyName = N'ThrottlingPublishOverrideSeverity' then CAST(@ThrottlingPublishOverrideSeverity as nvarchar)
           when PropertyName = N'ThrottlingDeliveryOverrideSeverity' then CAST(@ThrottlingDeliveryOverrideSeverity as nvarchar)
           when PropertyName = N'ThrottlingPublishOverride' then CAST(@ThrottlingPublishOverride as nvarchar)
           when PropertyName = N'ThrottlingDeliveryOverride' then CAST(@ThrottlingDeliveryOverride as nvarchar) 
           when PropertyName = N'DehydrationBehavior' then CAST(@DehydrationBehavior as nvarchar)
           when PropertyName = N'TimeBasedMaxThreshold' then CAST(@TimeBasedMaxThreshold as nvarchar) 
           when PropertyName = N'TimeBasedMinThreshold' then CAST(@TimeBasedMinThreshold as nvarchar)
           when PropertyName = N'SubscriptionPauseAt' then CAST(@SubscriptionPauseAt as nvarchar) 
           when PropertyName = N'SubscriptionResumeAt' then CAST(@SubscriptionResumeAt as nvarchar) 
         END 
         )
        where PropertyName IN (N'UseDefaultAppDomainForIsolatedAdapter', 
                               N'AllowMultipleResponses',
                               N'MessagingMaxReceiveInterval',
                               N'XlangMaxReceiveInterval',
                               N'MessagingReqRespTTL',
                               N'MsgAgentPerfCounterServiceClassID',
                               N'LegacyWhitespace',
                               N'ThrottlingSpoolMultiplier',
                               N'ThrottlingTrackingDataMultiplier',
                               N'ThrottlingLimitToTriggerGC',
                               N'ThrottlingBatchMemoryThresholdPercent',
                               N'ThrottlingSeverityProcessMemory',
                               N'ThrottlingSeverityDatabaseSize',
                               N'ThrottlingSeverityInflightMessage',
                               N'ThrottlingPublishOverrideSeverity',
                               N'ThrottlingDeliveryOverrideSeverity',
                               N'ThrottlingPublishOverride',
                               N'ThrottlingDeliveryOverride',
                               N'DehydrationBehavior',
                               N'TimeBasedMaxThreshold',
                               N'TimeBasedMinThreshold',
                               N'SubscriptionPauseAt',
                               N'SubscriptionResumeAt'
                               ) AND HostId = @HostId 
        
        -- Update adm_Host record
  update adm_Host
  set
   DateModified = GETUTCDATE(), 
   LastUsedLogon = @LastUsedLogon,
   HostTracking = @HostTracking,
   AuthTrusted = @AuthTrusted,
   DecryptCertName = @DecryptCertName,
   DecryptCertThumbprint = @DecryptCertThumbprint,
   NTGroupName = @NTGroupName,
   ClusterResourceGroupName = @ClusterResourceGroupName,
   IsHost32BitOnly = @IsHost32BitOnly,     
   MessageDeliverySampleSpaceSize = @MessageDeliverySampleSpaceSize,
   MessageDeliverySampleSpaceWindow = @MessageDeliverySampleSpaceWindow,
   MessageDeliveryOverdriveFactor = @MessageDeliveryOverdriveFactor,
   MessageDeliveryMaximumDelay = @MessageDeliveryMaximumDelay,
   MessagePublishSampleSpaceSize = @MessagePublishSampleSpaceSize,
   MessagePublishSampleSpaceWindow = @MessagePublishSampleSpaceWindow,
   MessagePublishOverdriveFactor = @MessagePublishOverdriveFactor,
   MessagePublishMaximumDelay = @MessagePublishMaximumDelay,
   DeliveryQueueSize = @DeliveryQueueSize,
   DBSessionThreshold = @DBSessionThreshold,
   GlobalMemoryThreshold = @GlobalMemoryThreshold,
   ProcessMemoryThreshold = @ProcessMemoryThreshold,
   ThreadThreshold = @ThreadThreshold,
   DBQueueSizeThreshold = @DBQueueSizeThreshold,
   InflightMessageThreshold = @InflightMessageThreshold,
   ThreadPoolSize = @ThreadPoolSize
  where
   Id = @HostId
  -- Reassign default host when "DefaultHost=true"
  if (0 <> @DefaultHost)
  begin
   update adm_Group set DefaultHostId = @HostId
  end
  -- at least one host in the group must host tracking 
  if not exists (select * from adm_Host where 0 <> HostTracking )
  begin
   set @ErrCode = 0xC0C0255B -- CIS_E_ADMIN_NO_APP_HOST_TRACKING
   goto exit_proc
  end
  --TDDS Configuration
   --check if host tracking flag is being changed in this call:
  if (@UsedToHostTracking <> @HostTracking)
  begin
   exec @ErrCode = adm_Host_Register_TDDS_Services @Name, @HostTracking
   if ( @ErrCode <> 0 ) goto exit_proc
  end
  
exit_proc:
 if(0 <> @bHasOpenedTransaction)
 begin
  if(@ErrCode = 0)
   commit transaction
  else
   rollback transaction
 end
 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Host_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

