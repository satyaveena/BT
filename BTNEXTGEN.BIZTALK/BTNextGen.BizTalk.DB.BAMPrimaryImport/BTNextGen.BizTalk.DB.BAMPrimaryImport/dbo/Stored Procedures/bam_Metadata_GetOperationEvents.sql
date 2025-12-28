-- Get all events for a Operation
CREATE PROCEDURE [dbo].[bam_Metadata_GetOperationEvents]
(
    @operationID    INT
)
AS
    SELECT DISTINCT ArtifactType, StartTime 
    FROM dbo.[bam_Metadata_OperationEvents]
    WHERE OperationID = @operationID AND EndTime IS NOT NULL
    ORDER BY StartTime DESC