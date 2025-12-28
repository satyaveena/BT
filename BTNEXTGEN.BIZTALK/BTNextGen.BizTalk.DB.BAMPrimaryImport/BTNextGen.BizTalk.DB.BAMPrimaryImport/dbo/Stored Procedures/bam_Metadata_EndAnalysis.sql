CREATE PROCEDURE [dbo].[bam_Metadata_EndAnalysis]
(
    @cubeName NVARCHAR(256)
)
AS
        UPDATE [dbo].[bam_Metadata_AnalysisTasks] SET LastEndTime = GETUTCDATE()
        WHERE CubeName = @cubeName AND LastEndTime IS NULL