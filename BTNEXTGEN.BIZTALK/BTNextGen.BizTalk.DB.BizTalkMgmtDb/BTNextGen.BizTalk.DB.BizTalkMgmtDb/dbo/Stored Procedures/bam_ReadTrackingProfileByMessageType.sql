CREATE PROCEDURE [dbo].[bam_ReadTrackingProfileByMessageType]
@nvcMessageType nvarchar(2048),
@nvcPortName nvarchar(256),
@nDirection int
AS
declare @uidPortId uniqueidentifier,
	@uidSchemaId uniqueidentifier,
	@nRetCode int
set @nRetCode = 0
/******
nRetCode = 0 means success
nRetCode = 1 means an invalid schema name was passed in
nRetCode = 2 means an invalid port name was passed in
nRetCode = 3 means no port name was passed in (it is required)
**********/
if (@nvcPortName IS NOT NULL)
BEGIN
	IF (@nDirection = 0)
	BEGIN
		SELECT @uidPortId = uidGUID FROM bts_receiveport WHERE nvcName = @nvcPortName
		if (@@ROWCOUNT = 0)
		BEGIN
			SELECT @uidPortId = uidGUID FROM bts_sendport WHERE nvcName = @nvcPortName AND bTwoWay = 1
		END
	END
	ELSE
	BEGIN
		SELECT @uidPortId = uidGUID FROM bts_sendport WHERE nvcName = @nvcPortName
		IF (@@ROWCOUNT = 0)
		BEGIN
			SELECT @uidPortId = uidGUID FROM bts_receiveport WHERE nvcName = @nvcPortName AND bTwoWay = 1
		END
	END
	
	if (@uidPortId IS NULL)
		set @nRetCode = 2
END
else
	set @nRetCode = 3
if (@nRetCode = 0)
BEGIN
	if (@nvcMessageType IS NOT NULL)
	BEGIN
		SELECT td.ntxtData, null, tp.uidVersionId, tp.nMinorVersionId, td.nTrackPointId
		FROM bam_TrackPoints td
		JOIN bam_TrackingProfiles tp ON td.nProfileId = tp.nID
		WHERE td.nvcMsgType = @nvcMessageType AND td.uidPortId = @uidPortId AND td.nDirection = @nDirection
		UNION ALL
		SELECT td.ntxtData, null, tp.uidVersionId, tp.nMinorVersionId, td.nTrackPointId
		FROM bam_TrackPoints td
		JOIN bam_TrackingProfiles tp ON td.nProfileId = tp.nID
		WHERE td.nvcMsgType IS NULL AND td.uidPortId = @uidPortId AND td.nDirection = @nDirection
	END
	else
	BEGIN
		SELECT td.ntxtData, null, tp.uidVersionId, tp.nMinorVersionId, td.nTrackPointId
		FROM bam_TrackPoints td
		JOIN bam_TrackingProfiles tp ON td.nProfileId = tp.nID
		WHERE td.nvcMsgType IS NULL AND td.uidPortId = @uidPortId AND td.nDirection = @nDirection
	END
END 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ReadTrackingProfileByMessageType] TO [BTS_HOST_USERS]
    AS [dbo];

