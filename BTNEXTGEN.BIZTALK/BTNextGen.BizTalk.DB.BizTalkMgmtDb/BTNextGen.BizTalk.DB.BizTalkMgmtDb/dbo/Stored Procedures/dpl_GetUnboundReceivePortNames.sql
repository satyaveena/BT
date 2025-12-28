CREATE PROCEDURE [dbo].[dpl_GetUnboundReceivePortNames] 

AS
SELECT nvcName FROM bts_receiveport WHERE nID not in
 (SELECT nReceivePortID FROM bts_orchestration_port_binding WHERE (nReceivePortID is not null))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundReceivePortNames] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundReceivePortNames] TO [BTS_OPERATORS]
    AS [dbo];

