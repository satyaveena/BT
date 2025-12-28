-- Stored procs start here

CREATE PROCEDURE [dbo].[TDDS_InsertFailedTrackingData]
@ErrorMessage	ntext,
@dataImage	image,
@source		nvarchar(256),
@formatID	uniqueidentifier,
@DestinationID	tinyint

AS
	declare @currentTime datetime
	SET @currentTime = GETUTCDATE()

	INSERT INTO TDDS_FailedTrackingData
	( ErrMessage, DataImage, Source, FormatID, dtLogTime, DestinatioNID)
	VALUES
	( @ErrorMessage, @dataImage, @source, @formatID,@currentTime, @DestinationID )
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_InsertFailedTrackingData] TO [BAM_EVENT_WRITER]
    AS [dbo];

