-- Update bam_Insert_OperationEvent table
CREATE PROCEDURE [dbo].[bam_Metadata_EndOperationEvent]
(
    @operationID    INT,
    @artifactType    NVARCHAR(30)
)
AS
    UPDATE dbo.[bam_Metadata_OperationEvents]
    SET EndTime = GETUTCDATE()
    WHERE OperationID = @operationID AND ArtifactType = @artifactType