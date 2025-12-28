CREATE PROCEDURE [dbo].[MsgBoxPerfCounters_GetMsgBoxesAndServers] AS

--SELECT DBServerName, DBName FROM adm_MessageBox WITH (NOLOCK)
-- Trying to get AppLock so as to serialize access in case two hosts - in different processes - are trying to access the same info at the same time.
DECLARE @retVal smallint

EXEC  @retVal = sp_getapplock @Resource = 'MsgBoxPerfCounters_adm_MessageBoxTable', @LockMode = 'Shared', @LockOwner = 'Session'

IF (@retVal = 0 ) -- Lock granted
BEGIN 
 SELECT DBServerName, DBName FROM adm_MessageBox
 EXEC sp_releaseapplock @Resource = 'MsgBoxPerfCounters_adm_MessageBoxTable', @LockOwner = 'Session'
END
 -- Else there was a problem getting lock and nothing is returned. Means that things will be considered to be 'deleted' from counters updating point of view.
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MsgBoxPerfCounters_GetMsgBoxesAndServers] TO [BTS_HOST_USERS]
    AS [dbo];

