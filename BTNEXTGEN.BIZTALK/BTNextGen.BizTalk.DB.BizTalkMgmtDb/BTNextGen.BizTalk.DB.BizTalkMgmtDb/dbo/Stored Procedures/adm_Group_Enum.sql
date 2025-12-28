CREATE PROCEDURE [dbo].[adm_Group_Enum]
AS
 set nocount on
 set xact_abort on
 select
  top 1
  Id,
  SERVERPROPERTY('ServerName'),
  DB_NAME(),
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
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Enum] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_Enum] TO [BTS_OPERATORS]
    AS [dbo];

