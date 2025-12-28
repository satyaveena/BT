CREATE   Procedure [dbo].[edi_FindTrackingActivityIdByCorrelationId]
	@correlationId nvarchar(255)
AS
BEGIN
	SELECT TOP 1    
				ActivityID as TrackingActivityID,
				BTSInterchangeID as InterchangeID,
				MessageContentId as MessageContentId
	FROM 		dbo.[bam_ResendTrackingActivity_ActiveInstances]
	WHERE		CorrelationId = @correlationId
	ORDER BY	LastModified DESC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_FindTrackingActivityIdByCorrelationId] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_FindTrackingActivityIdByCorrelationId] TO [BTS_OPERATORS]
    AS [dbo];

