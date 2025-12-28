CREATE PROCEDURE [dbo].[bam_Metadata_GetAllActivities]
AS
    SELECT  ActivityName
    FROM    [dbo].[bam_Metadata_Activities]