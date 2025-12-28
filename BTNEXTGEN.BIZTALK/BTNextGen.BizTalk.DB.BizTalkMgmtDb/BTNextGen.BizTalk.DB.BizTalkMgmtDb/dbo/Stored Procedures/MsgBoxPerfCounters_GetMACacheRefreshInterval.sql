CREATE PROCEDURE [dbo].[MsgBoxPerfCounters_GetMACacheRefreshInterval] AS

--SELECT ConfigurationCacheRefreshInterval FROM adm_Group WITH (NOLOCK)
-- Trying to get AppLock so as to serialize access in case two hosts (in different processes) are trying to access the same info at the same time.
DECLARE @retVal smallint

EXEC  @retVal = sp_getapplock @Resource = 'MsgBoxPerfCounters_adm_GroupTable', @LockMode = 'Shared', @LockOwner = 'Session'

IF (@retVal = 0 ) -- Lock granted
BEGIN 
 SELECT ConfigurationCacheRefreshInterval FROM adm_Group
 EXEC sp_releaseapplock @Resource = 'MsgBoxPerfCounters_adm_GroupTable', @LockOwner = 'Session'
END
ELSE
 -- Else there was a problem getting lock - return the default value of 60 in this case
BEGIN
 SELECT 60
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MsgBoxPerfCounters_GetMACacheRefreshInterval] TO [BTS_HOST_USERS]
    AS [dbo];

