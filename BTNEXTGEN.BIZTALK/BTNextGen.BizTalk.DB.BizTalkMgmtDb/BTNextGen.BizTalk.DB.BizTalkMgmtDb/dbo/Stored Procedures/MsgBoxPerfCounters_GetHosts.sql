CREATE PROCEDURE [dbo].[MsgBoxPerfCounters_GetHosts] AS

--SELECT Name FROM adm_Host WITH (NOLOCK)
-- Trying to get AppLock so as to serialize access in case two hosts (in different processes) are trying to access the same info at the same time.
DECLARE @retVal smallint

EXEC  @retVal = sp_getapplock @Resource = 'MsgBoxPerfCounters_adm_HostTable', @LockMode = 'Shared', @LockOwner = 'Session'

IF (@retVal = 0 ) -- Lock granted
BEGIN 
 SELECT Name FROM adm_Host
 EXEC sp_releaseapplock @Resource = 'MsgBoxPerfCounters_adm_HostTable', @LockOwner = 'Session'
END
 -- Else there was a problem getting lock and nothing is returned.
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MsgBoxPerfCounters_GetHosts] TO [BTS_HOST_USERS]
    AS [dbo];

