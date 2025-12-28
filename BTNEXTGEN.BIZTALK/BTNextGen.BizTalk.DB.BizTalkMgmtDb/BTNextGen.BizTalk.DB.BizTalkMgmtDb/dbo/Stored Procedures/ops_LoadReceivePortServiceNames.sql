CREATE PROCEDURE [dbo].[ops_LoadReceivePortServiceNames]
AS

set transaction isolation level read committed
set nocount on
set deadlock_priority low

SELECT uidGUID, nvcName FROM bts_receiveport WITH (ROWLOCK READPAST)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadReceivePortServiceNames] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_LoadReceivePortServiceNames] TO [BTS_OPERATORS]
    AS [dbo];

