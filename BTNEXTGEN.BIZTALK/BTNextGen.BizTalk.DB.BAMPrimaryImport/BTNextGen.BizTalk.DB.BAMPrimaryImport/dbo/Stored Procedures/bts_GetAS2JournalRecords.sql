CREATE  PROCEDURE [dbo].[bts_GetAS2JournalRecords]
(
	@TrackingActivityID	nvarchar(256)
) AS
BEGIN 

	SELECT 
		ResendIndex,
		ResendStatus,
		LastModified as ResendTime
	FROM 
		bam_ResendJournalActivity_CompletedInstances 						
	WHERE 
		TrackingActivityID = @TrackingActivityID
	
END

GRANT EXECUTE ON [dbo].[bts_GetAS2JournalRecords] TO [BTS_ADMIN_USERS]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetAS2JournalRecords] TO [BTS_OPERATORS]
    AS [dbo];

