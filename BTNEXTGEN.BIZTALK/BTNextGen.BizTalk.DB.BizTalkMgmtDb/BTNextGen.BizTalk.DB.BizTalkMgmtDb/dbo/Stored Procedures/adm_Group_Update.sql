CREATE PROCEDURE [dbo].[adm_Group_Update]
@MgmtDBServerName nvarchar(80),
@MgmtDBName nvarchar(128),
@Name nvarchar(256),
@TrackingDBServerName nvarchar(80),
@TrackingDBName nvarchar(128),
@SubscriptionDBServerName nvarchar(80),
@SubscriptionDBName nvarchar(128),
@TrackAnalysisServerName nvarchar(80),
@TrackAnalysisDBName nvarchar(128),
@BamDBServerName nvarchar(80),
@BamDBName nvarchar(128),
@RuleEngineDBServerName nvarchar(80),
@RuleEngineDBName nvarchar(128),
@SSOServerName nvarchar(80),
@GlobalTrackingOption int,
@SignCertName nvarchar(256),
@SignCertThumbprint nvarchar(80),
@ConfigurationCacheRefreshInterval int,
@BizTalkAdminGroup nvarchar(128), -- this property actually cannot be updated
@LMSFragmentSize int,
@LMSThreshold int,
@BizTalkOperatorGroup nvarchar(128),
@GroupPropertyIdentifier nvarchar(128),  -- this property actually cannot be updated
@PerfCounterCacheRefreshInterval int,
@BizTalkB2BOperatorGroup nvarchar(128)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 set @ErrCode = 0
 
 begin transaction
  update adm_Group
  set
   Name = @Name,
   TrackingDBServerName = @TrackingDBServerName, 
   TrackingDBName = @TrackingDBName, 
   SubscriptionDBServerName = @SubscriptionDBServerName, 
   SubscriptionDBName = @SubscriptionDBName, 
   TrackAnalysisServerName = @TrackAnalysisServerName, 
   TrackAnalysisDBName = @TrackAnalysisDBName, 
   BamDBServerName = @BamDBServerName, 
   BamDBName = @BamDBName, 
   RuleEngineDBServerName = @RuleEngineDBServerName, 
   RuleEngineDBName = @RuleEngineDBName, 
   SSOServerName = @SSOServerName,
   GlobalTrackingOption = @GlobalTrackingOption,
   DateModified = GETUTCDATE(), 
   SignCertName = @SignCertName,
   SignCertThumbprint = @SignCertThumbprint,
   ConfigurationCacheRefreshInterval = @ConfigurationCacheRefreshInterval,
   LMSFragmentSize = @LMSFragmentSize,
   LMSThreshold = @LMSThreshold,
   BizTalkOperatorGroup = @BizTalkOperatorGroup,
   BizTalkB2BOperatorGroup = @BizTalkB2BOperatorGroup
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  -- Update the GroupSetting table  
  update adm_GroupSetting SET PropertyValue = 
  (CASE
           when PropertyName = N'PerfCounterCacheRefreshInterval' then CAST(@PerfCounterCacheRefreshInterval as nvarchar) 
         END 
         )
        where PropertyName IN (N'PerfCounterCacheRefreshInterval')

     -- Update the Master MsgBox record
  update adm_MessageBox
  set
   DateModified = GETUTCDATE(), 
   DBServerName = @SubscriptionDBServerName,
   DBName = @SubscriptionDBName
  where
   IsMasterMsgBox <> 0
   
  --configure TDDS:
  --declare @GroupName nvarchar(256)
  --set @GroupName = dbo.adm_GetGroupName()
  --exec @ErrCode = TDDS_UpdatePoolRefreshInterval @ConfigurationCacheRefreshInterval
  --if ( @ErrCode <> 0 ) goto exit_proc
exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

