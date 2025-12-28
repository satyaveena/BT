CREATE PROCEDURE [dbo].[bam_DeployTrackPoint]
	@nTrackPointId int,
	@nvcSchemaName nvarchar(513),
	@nvcPortName nvarchar(256),
	@nDirection int,
	@uidProfileVersionId uniqueidentifier,
	@nMinorVersionId int,
	@ntxtData ntext
AS
BEGIN
	DECLARE @nvcMsgType nvarchar(2048),
		@uidPortId uniqueidentifier,
		@pipelineRelease int,
		@nProfileId int
	declare @localized_Invalid_Pipeline_Type AS nvarchar(256)
	set @localized_Invalid_Pipeline_Type  = N'Trackpoints cannot be associated with ports which use custom pipelines upgraded from BizTalk 2004'
	declare @localized_Invalid_Port_Name AS nvarchar(256)
	set @localized_Invalid_Port_Name = N'The port name associated with this trackpoint could not be found'
	
	IF (@nvcSchemaName IS NOT NULL)
	BEGIN
		--there could be versioning, but the docspec name, msgtype pairing shouldn't change with versioning
		SELECT TOP 1 @nvcMsgType = msgtype FROM bt_DocumentSpec WHERE docspec_name = @nvcSchemaName 
	END
	IF (@nvcPortName IS NOT NULL)
	BEGIN
		--if there is a port, we need to check to make sure it does not use a voyager pipeline. For sendports
		--this check is easy since the pipeline is tied to the port. For receive ports, if any receive location 
		--associated with this port uses a voyager pipeline, we will not allow it since we don't know which location
		--the message could come in via. Users should just create a new port if they can't update the pipeline		
		IF (@nDirection = 0)
		BEGIN
			SELECT TOP 1 @uidPortId = rp.uidGUID, @pipelineRelease = p.Release FROM bts_receiveport rp
				JOIN adm_ReceiveLocation rl ON rl.ReceivePortId = rp.nID
				LEFT JOIN bts_pipeline p ON rl.ReceivePipelineId = p.Id AND p.Release = 1
			WHERE nvcName = @nvcPortName
			if (@@ROWCOUNT = 0)
			BEGIN
				SELECT @pipelineRelease = p.Release, @uidPortId = s.uidGUID FROM bts_sendport s
				JOIN bts_pipeline p ON s.nReceivePipelineID = p.Id
				WHERE s.nvcName = @nvcPortName AND s.bTwoWay = 1
			END
		END
		ELSE
		BEGIN
			SELECT @pipelineRelease = p.Release, @uidPortId = s.uidGUID FROM bts_sendport s
				JOIN bts_pipeline p ON s.nSendPipelineID = p.Id
				WHERE s.nvcName = @nvcPortName
			IF (@@ROWCOUNT = 0)
			BEGIN
				SELECT TOP 1 @uidPortId = rp.uidGUID, @pipelineRelease = p.Release FROM bts_receiveport rp
					JOIN adm_ReceiveLocation rl ON rl.ReceivePortId = rp.nID
					LEFT JOIN bts_pipeline p ON rl.SendPipelineId = p.Id AND p.Release = 1
				WHERE nvcName = @nvcPortName AND bTwoWay = 1
			END
		END
		if (@uidPortId IS NULL)
		BEGIN
			RAISERROR(@localized_Invalid_Port_Name, 16, 1)
			return
		END
		if ( (@pipelineRelease IS NOT NULL) AND (@pipelineRelease = 1) )
		BEGIN
			RAISERROR(@localized_Invalid_Pipeline_Type, 16, 1)
			return
		END
	END
	SELECT @nProfileId = nID FROM bam_TrackingProfiles WHERE uidVersionId = @uidProfileVersionId AND nMinorVersionId = @nMinorVersionId
	
	INSERT INTO bam_TrackPoints (nTrackPointId, nvcMsgType, uidPortId, nDirection, nProfileId, ntxtData)
		VALUES (@nTrackPointId, @nvcMsgType, @uidPortId, @nDirection, @nProfileId, @ntxtData)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_DeployTrackPoint] TO [BTS_ADMIN_USERS]
    AS [dbo];

