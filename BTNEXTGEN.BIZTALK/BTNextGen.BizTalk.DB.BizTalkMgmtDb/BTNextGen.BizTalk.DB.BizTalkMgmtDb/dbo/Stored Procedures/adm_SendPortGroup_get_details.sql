CREATE PROCEDURE [dbo].[adm_SendPortGroup_get_details]
@uidSendPortGroup uniqueidentifier
AS
set nocount on
set transaction isolation level read committed
SELECT TOP 1 spg.nvcName, spg.nvcFilter, h.Name, g.Name
FROM bts_sendportgroup spg, adm_Group g
JOIN adm_Host h ON g.DefaultHostId = h.Id
WHERE spg.uidGUID = @uidSendPortGroup

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPortGroup_get_details] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPortGroup_get_details] TO [BTS_OPERATORS]
    AS [dbo];

