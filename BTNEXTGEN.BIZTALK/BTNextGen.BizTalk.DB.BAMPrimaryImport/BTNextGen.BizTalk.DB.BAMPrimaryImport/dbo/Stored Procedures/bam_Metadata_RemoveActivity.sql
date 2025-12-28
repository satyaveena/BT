CREATE PROCEDURE [dbo].[bam_Metadata_RemoveActivity]
(
    @activityName NVARCHAR(128)
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    -- Remove activity from the metadata table
    DELETE 
    FROM [dbo].[bam_Metadata_Activities]
    WHERE ActivityName = @activityName

    -- Remove any existing Custom Indexes
    DELETE 
    FROM dbo.bam_Metadata_CustomIndexes 
    WHERE ActivityName = @activityName

    -- Remove any partition info related to this activity
    DELETE 
    FROM dbo.bam_Metadata_Partitions 
    WHERE ActivityName = @activityName