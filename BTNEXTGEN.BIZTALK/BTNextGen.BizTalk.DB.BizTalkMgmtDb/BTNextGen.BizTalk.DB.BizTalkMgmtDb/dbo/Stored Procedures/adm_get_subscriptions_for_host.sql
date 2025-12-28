CREATE PROCEDURE [dbo].[adm_get_subscriptions_for_host]
@nvcAdapterName nvarchar(256),
@nvcHostName nvarchar(80),
@bGetDynamicPorts bit,
@bGetStaticPorts bit,
@nvcGroupName nvarchar(256) OUTPUT,
@nCount int OUTPUT
AS
declare @nAdapterID int,
	@nSendHandlerID int
SELECT @nAdapterID = a.Id, @nSendHandlerID = sh.Id, @nvcGroupName = g.Name
FROM adm_Adapter a
JOIN adm_SendHandler sh
	JOIN adm_Group g ON sh.GroupId = g.Id
	JOIN adm_Host h ON sh.HostId = h.Id AND h.Name = @nvcHostName
  ON a.Id = sh.AdapterId
WHERE a.Name = @nvcAdapterName  
if (@@ROWCOUNT = 0)
BEGIN
	set @nCount = -1
	return
END
SELECT uidGUID 
FROM bts_sendport_transport spt
WHERE spt.nTransportTypeId = @nAdapterID AND @bGetStaticPorts = 1 AND spt.nSendHandlerID = @nSendHandlerID
UNION
SELECT spgs.uidPrimaryGUID as uidGUID
FROM bts_spg_sendport spgs
JOIN bts_sendport_transport spt ON spgs.nSendPortID = spt.nID AND spt.bIsPrimary = 1 AND spt.nTransportTypeId = @nAdapterID AND spt.nSendHandlerID = @nSendHandlerID
WHERE @bGetStaticPorts = 1
UNION
SELECT dps.uidGUID 
FROM bts_dynamicport_subids dps
WHERE dps.nSendHandlerID = @nSendHandlerID AND @bGetDynamicPorts = 1
set @nCount = @@ROWCOUNT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_get_subscriptions_for_host] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_get_subscriptions_for_host] TO [BTS_OPERATORS]
    AS [dbo];

