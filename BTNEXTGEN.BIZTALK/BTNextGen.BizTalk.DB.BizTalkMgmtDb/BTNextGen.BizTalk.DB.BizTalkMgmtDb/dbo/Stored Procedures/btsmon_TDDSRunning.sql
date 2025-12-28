CREATE PROCEDURE [dbo].[btsmon_TDDSRunning]
@nIssues bigint output
AS
 --Check if Global tracking is enabled and TDDS is not running on any Host instance 
 set @nIssues = 0
 declare @GlobalTracking int
 SELECT @GlobalTracking = GlobalTrackingOption FROM [dbo].[adm_Group]
 if (@GlobalTracking = 1)
 BEGIN
  declare @LastHeartBeat DateTime
  declare @SessionTimeout int
  SELECT @LastHeartBeat = MAX(TimeLastChanged) FROM TDDS_Heartbeats
  SELECT @SessionTimeout = SessionTimeout FROM TDDS_Settings
  if ((@LastHeartBeat is NULL) OR (DATEDIFF(SS,  @LastHeartBeat, GETUTCDATE()) > @SessionTimeout))
   set @nIssues = 1
 END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsmon_TDDSRunning] TO [BTS_ADMIN_USERS]
    AS [dbo];

