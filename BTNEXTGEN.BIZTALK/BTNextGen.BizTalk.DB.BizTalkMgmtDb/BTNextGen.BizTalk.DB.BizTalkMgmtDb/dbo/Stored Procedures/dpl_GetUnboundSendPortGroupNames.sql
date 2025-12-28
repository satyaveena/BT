CREATE PROCEDURE [dbo].[dpl_GetUnboundSendPortGroupNames] 

AS
SELECT nvcName FROM bts_sendportgroup WHERE nID not in
 (SELECT nSpgID FROM bts_orchestration_port_binding WHERE (nSpgID is not null))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundSendPortGroupNames] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetUnboundSendPortGroupNames] TO [BTS_OPERATORS]
    AS [dbo];

