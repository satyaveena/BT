-- Insert into bam_OperationEvents table
CREATE PROCEDURE [dbo].[bam_Metadata_BeginOperationEvent]
(
    @operationID        INT,
    @artifactType        NVARCHAR(30)
)
AS
    INSERT dbo.[bam_Metadata_OperationEvents]    (OperationID, StartTime, ArtifactType)
    VALUES (@operationID, GETUTCDATE(), @artifactType)