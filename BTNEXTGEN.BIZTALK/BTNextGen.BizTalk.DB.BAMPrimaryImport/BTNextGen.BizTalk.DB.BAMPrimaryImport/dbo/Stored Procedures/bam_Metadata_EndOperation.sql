-- Update bam_Operations table
CREATE PROCEDURE [dbo].[bam_Metadata_EndOperation]
(
    @operationID    INT
)
AS
    UPDATE dbo.[bam_Metadata_Operations]
    SET EndTime = GETUTCDATE()
    WHERE OperationID = @operationID