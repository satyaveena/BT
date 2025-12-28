CREATE PROCEDURE [dbo].[ops_LoadSendPortSpecificFields]
@uidPortID uniqueidentifier
AS

set transaction isolation level read committed
set nocount on
set deadlock_priority low

SELECT st.bIsPrimary, st.bIsServiceWindow, st.dtFromTime, st.dtToTime, sp.nPriority, st.bOrderedDelivery, sp.bStopSendingOnFailure, sp.bRouteFailedMessage
FROM bts_sendport AS sp
LEFT OUTER JOIN bts_sendport_transport AS st ON (sp.nID = st.nSendPortID)
WHERE st.uidGUID = @uidPortID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadSendPortSpecificFields] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadSendPortSpecificFields] TO [BTS_OPERATORS]
    AS [dbo];

