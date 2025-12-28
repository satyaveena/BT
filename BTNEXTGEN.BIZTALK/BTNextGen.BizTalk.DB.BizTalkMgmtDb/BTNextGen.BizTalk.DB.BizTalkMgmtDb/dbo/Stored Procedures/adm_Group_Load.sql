CREATE PROCEDURE [dbo].[adm_Group_Load]
@MgmtDBServerName nvarchar(80),
@MgmtDBName nvarchar(128)
AS
 set nocount on
 set xact_abort on
 declare @ErrCode as int
 set @ErrCode = 0
 select
  top 1
  Id,
  @MgmtDBServerName,
  @MgmtDBName,
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
  DateModified, 
  ConfigurationCacheRefreshInterval,
  BizTalkAdminGroup,
  BizTalkDBVersion.DatabaseMajor,
  BizTalkDBVersion.DatabaseMinor,
  LMSFragmentSize,
  LMSThreshold,
  BizTalkOperatorGroup,
  GroupPropertyIdentifier,
  PerfCounterCacheRefreshInterval,
  BizTalkB2BOperatorGroup
 from
  adm_Group,
  BizTalkDBVersion,
  (select GroupId,
           MAX(case when PropertyName= N'PerfCounterCacheRefreshInterval' then CAST(PropertyValue as int) END) as PerfCounterCacheRefreshInterval
         From adm_GroupSetting
         Group BY GroupId) temp
 where
  BizTalkDBVersion.BizTalkDBName = N'Management DB' AND adm_Group.Id = temp.GroupId
 order
  by BizTalkDBVersion.DatabaseMajor, BizTalkDBVersion.DatabaseMinor DESC
 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode
 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Load] TO [BTS_OPERATORS]
    AS [dbo];

