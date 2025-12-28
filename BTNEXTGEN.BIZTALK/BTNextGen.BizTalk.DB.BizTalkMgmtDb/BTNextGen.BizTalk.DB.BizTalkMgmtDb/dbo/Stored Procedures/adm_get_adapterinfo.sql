CREATE PROCEDURE [dbo].[adm_get_adapterinfo]
@uidSendPortID uniqueidentifier,
@nvcGroupName nvarchar(256) OUTPUT,
@nvcHostName nvarchar(80) OUTPUT
AS
SELECT @nvcGroupName = g.Name, @nvcHostName = h.Name
FROM bts_sendport sp
JOIN bts_sendport_transport spt 
	JOIN adm_SendHandler sh
		JOIN adm_Host h ON sh.HostId = h.Id
		JOIN adm_Group g ON sh.GroupId = g.Id
	ON spt.nSendHandlerID = sh.Id
ON sp.nID = spt.nSendPortID AND spt.bIsPrimary = 1
WHERE sp.uidGUID = @uidSendPortID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_get_adapterinfo] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_get_adapterinfo] TO [BTS_OPERATORS]
    AS [dbo];

