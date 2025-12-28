CREATE PROCEDURE [dbo].[adm_spgp_get_details]
@uidSPGP uniqueidentifier
AS
SELECT spg.nvcName, spg.uidGUID, spg.nPortStatus, sp.uidGUID, sp.nvcName, CAST(sp.bTwoWay as int) AS bTwoWay, sp.nPortStatus, sp.nPriority, h.Name, g.Name, ad.Name, ad.OutboundEngineCLSID, spgp.uidPrimaryGUID, spt.uidGUID, CAST (spt.bIsServiceWindow AS int) AS bIsServiceWindow, [dbo].[adm_fnConvertLocalToUTCDate](spt.dtFromTime), [dbo].[adm_fnConvertLocalToUTCDate](spt.dtToTime), spt.nvcAddress, CAST (spt.bOrderedDelivery AS int) AS bOrderedDelivery
FROM bts_spg_sendport spgp 
JOIN bts_sendportgroup spg ON spgp.nSendPortGroupID = spg.nID
JOIN bts_sendport sp
	JOIN bts_sendport_transport spt 
		LEFT JOIN adm_SendHandler sh 
			JOIN adm_Adapter ad ON ad.Id = sh.AdapterId
			JOIN adm_Host h ON sh.HostId = h.Id
			JOIN adm_Group g ON sh.GroupId = g.Id
		ON spt.nSendHandlerID = sh.Id
	ON sp.nID = spt.nSendPortID AND spt.bIsPrimary = 1
ON sp.nID = spgp.nSendPortID
WHERE spgp.uidPrimaryGUID = @uidSPGP

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_spgp_get_details] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_spgp_get_details] TO [BTS_OPERATORS]
    AS [dbo];

