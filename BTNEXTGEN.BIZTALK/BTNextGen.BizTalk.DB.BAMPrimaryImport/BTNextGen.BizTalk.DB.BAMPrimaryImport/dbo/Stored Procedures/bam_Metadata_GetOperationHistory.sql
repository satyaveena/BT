CREATE PROCEDURE [dbo].[bam_Metadata_GetOperationHistory]
AS
    SELECT OperationID, UserLogin, OperationType, StartTime, 
           OriginalDefinitionXml, BamDefinitionFileName, BamManagerFileVersion
    FROM dbo.bam_Metadata_Operations WHERE EndTime IS NOT NULL
    ORDER BY OperationID ASC