CREATE PROCEDURE [dbo].[dpl_GetUnboundSendPortNames] 

AS
SELECT nvcName FROM bts_sendport WHERE nID not in
 (SELECT nSendPortID FROM bts_orchestration_port_binding WHERE (nSendPortID is not null))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundSendPortNames] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundSendPortNames] TO [BTS_OPERATORS]
    AS [dbo];

