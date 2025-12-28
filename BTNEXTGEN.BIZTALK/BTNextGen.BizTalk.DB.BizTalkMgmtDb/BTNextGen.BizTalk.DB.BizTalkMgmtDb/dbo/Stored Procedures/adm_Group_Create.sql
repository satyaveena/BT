CREATE PROCEDURE [dbo].[adm_Group_Create]
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
@BizTalkAdminGroup nvarchar(128),
@LMSFragmentSize int,
@LMSThreshold int,
@BizTalkOperatorGroup nvarchar(128),
@GroupPropertyIdentifier nvarchar(128),
@PerfCounterCacheRefreshInterval int,
@BizTalkB2BOperatorGroup nvarchar(128)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 declare @GroupId as int
 set @ErrCode = 0
  
 begin transaction
  insert into adm_Group
  (
   Name, 
   TrackingDBServerName, 
   TrackingDBName, 
   SubscriptionDBServerName, 
   SubscriptionDBName, 
   TrackAnalysisServerName, 
   TrackAnalysisDBName, 
   BamDBServerName, 
   BamDBName, 
   RuleEngineDBServerName, 
   RuleEngineDBName, 
   SSOServerName,
   GlobalTrackingOption,
   SignCertName,
   SignCertThumbprint,
   ConfigurationCacheRefreshInterval,
   BizTalkAdminGroup,
   LMSFragmentSize,
   LMSThreshold,
   BizTalkOperatorGroup,
   GroupPropertyIdentifier,
   BizTalkB2BOperatorGroup
  )
  values
  (
   @Name, 
   @TrackingDBServerName, 
   @TrackingDBName, 
   @SubscriptionDBServerName, 
   @SubscriptionDBName, 
   @TrackAnalysisServerName,
   @TrackAnalysisDBName,
   @BamDBServerName, 
   @BamDBName, 
   @RuleEngineDBServerName, 
   @RuleEngineDBName, 
   @SSOServerName,
   @GlobalTrackingOption,
   @SignCertName,
   @SignCertThumbprint,
   @ConfigurationCacheRefreshInterval,
   @BizTalkAdminGroup,
   @LMSFragmentSize,
   @LMSThreshold,
   @BizTalkOperatorGroup,
   @GroupPropertyIdentifier,
   @BizTalkB2BOperatorGroup
  )
  set @GroupId = @@IDENTITY
  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc
  insert into adm_GroupSetting (GroupId,PropertyName, PropertyValue)
  select @GroupId, N'PerfCounterCacheRefreshInterval', CAST(@PerfCounterCacheRefreshInterval as nvarchar)
    
  declare @localized_string_HM_1 as nvarchar(256)
  set @localized_string_HM_1 = N'Health Monitoring'
  declare @localized_string_BI_1 as nvarchar(256)
  set @localized_string_BI_1 = N'Business Monitoring'
  
  --configure TDDS:
  --Health Monitoring Destination
  exec @ErrCode = TDDS_CreateDBDestination @localized_string_HM_1, @TrackingDBServerName, @TrackingDBName
  if ( @ErrCode <> 0 ) goto exit_proc
  --Business Tracking Destination
  exec @ErrCode = TDDS_CreateDBDestination @localized_string_BI_1, @TrackingDBServerName, @TrackingDBName
  if ( @ErrCode <> 0 ) goto exit_proc
  -- add the MsgBox record for the Subscription DB (Master MB)
  exec @ErrCode = dbo.adm_MessageBox_Create @SubscriptionDBServerName, @SubscriptionDBName, 0, 1, '' -- eMSGBOX_CONFIGURATION_DONE
  if ( @ErrCode <> 0 ) goto exit_proc
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
    ON OBJECT::[dbo].[adm_Group_Create] TO [BTS_ADMIN_USERS]
    AS [dbo];

