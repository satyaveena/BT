CREATE PROCEDURE [dbo].[adm_SendPort_get_subscription_details]
@uidSendPort uniqueidentifier
AS
set nocount on
set transaction isolation level read committed
declare @nSendPortID int,
	@fIsDynamic bit
SELECT @nSendPortID = nID, @fIsDynamic = bDynamic
FROM bts_sendport
WHERE uidGUID = @uidSendPort
if (@fIsDynamic = 0)
BEGIN
	SELECT sp.nvcName, sp.nvcFilter, sp.nPortStatus, CAST(sp.bDynamic as int) AS bDynamic, CAST(sp.bTwoWay as int) AS bTwoWay, CAST(spt.bIsPrimary as int) AS bIsPrimary, sp.nPriority, h.Name, g.Name, ad.Name, ad.OutboundEngineCLSID, spt.uidGUID, CAST (spt.bIsServiceWindow AS int) AS bIsServiceWindow, [dbo].[adm_fnConvertLocalToUTCDate](spt.dtFromTime), [dbo].[adm_fnConvertLocalToUTCDate](spt.dtToTime), spt.nvcAddress, CAST (spt.bOrderedDelivery AS int) AS bOrderedDelivery
	FROM bts_sendport sp
	JOIN bts_sendport_transport spt 
		LEFT JOIN adm_SendHandler sh 
			JOIN adm_Adapter ad ON sh.AdapterId = ad.Id
			JOIN adm_Host h ON sh.HostId = h.Id
			JOIN adm_Group g ON sh.GroupId = g.Id
		ON spt.nSendHandlerID = sh.Id
	ON sp.nID = spt.nSendPortID
	WHERE sp.uidGUID = @uidSendPort
	ORDER BY bIsPrimary DESC
		
END
ELSE
BEGIN
	--lets make sure that we have subscription ids for all necessary send handlers
	INSERT INTO bts_dynamicport_subids (uidSendPortID, nSendHandlerID, nvcHostName)
	SELECT @uidSendPort, sh.Id, h.Name
	FROM adm_SendHandler sh
	JOIN adm_Host h ON sh.HostId = h.Id
	WHERE sh.IsDefault != 0 AND sh.Id NOT IN (SELECT nSendHandlerID FROM bts_dynamicport_subids WHERE uidSendPortID = @uidSendPort)
	SELECT sp.nvcName, sp.nvcFilter, sp.nPortStatus, 1, CAST(sp.bTwoWay as int) AS bTwoWay, 1, sp.nPriority,
		CASE WHEN (h.Name) IS NULL THEN dps.nvcHostName ELSE h.Name END AS Name,
		g.Name, ad.Name, ad.OutboundEngineCLSID, dps.uidGUID, 0, 0, 0, NULL, 0
	FROM bts_sendport sp
	JOIN bts_dynamicport_subids dps ON dps.uidSendPortID = sp.uidGUID
	LEFT JOIN adm_SendHandler sh
		JOIN adm_Adapter ad ON ad.Id = sh.AdapterId
		JOIN adm_Host h ON sh.HostId = h.Id
		JOIN adm_Group g ON sh.GroupId = g.Id
	ON dps.nSendHandlerID = sh.Id
	WHERE sp.uidGUID = @uidSendPort
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPort_get_subscription_details] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendPort_get_subscription_details] TO [BTS_OPERATORS]
    AS [dbo];

