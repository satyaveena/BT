-- Pick a partition to be archived and reconstruct archiving views
CREATE PROCEDURE [dbo].[bam_Metadata_BeginArchiving]
(
    @activityName NVARCHAR(256)
)
AS
    BEGIN TRAN

    DECLARE @@OnlineWindowTimeUnit CHAR(10)
    DECLARE @@OnlineWindowTimeLength INT
    
    -- Pause the primary import until this SP is executed
    -- and get current activity's online window
    SELECT        @@OnlineWindowTimeUnit = UPPER(OnlineWindowTimeUnit),
            @@OnlineWindowTimeLength = OnlineWindowTimeLength
     FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, XLOCK) WHERE ActivityName = @activityName
     
    IF @@ROWCOUNT = 0
    BEGIN
        -- Current activity has never registered
        RAISERROR (N'BeginArchiving_NoActivityForArchiving', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN 255
    END

    -- If previous archiving run is still in progress, continue last run
    SELECT ActivityName FROM [dbo].[bam_Metadata_Partitions]
    WHERE ArchivingInProgress = 1 AND ActivityName = @activityName
    IF @@ROWCOUNT > 0
    BEGIN
        COMMIT
        RETURN 255
    END

    DECLARE @@OnlineThreshold DATETIME
    IF @@OnlineWindowTimeUnit = 'MONTH'
        SET @@OnlineThreshold = DATEADD(MONTH, -@@OnlineWindowTimeLength, GETUTCDATE())
    ELSE IF @@OnlineWindowTimeUnit = 'DAY'
        SET @@OnlineThreshold = DATEADD(DAY, -@@OnlineWindowTimeLength, GETUTCDATE())
    ELSE IF @@OnlineWindowTimeUnit = 'HOUR'
        SET @@OnlineThreshold = DATEADD(HOUR, -@@OnlineWindowTimeLength, GETUTCDATE())
    ELSE IF @@OnlineWindowTimeUnit = 'MINUTE' 
        SET @@OnlineThreshold = DATEADD(MINUTE, -@@OnlineWindowTimeLength, GETUTCDATE())
    ELSE
    BEGIN
        RAISERROR (N'BeginArchiving_TimeUnitValues', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN 255
    END
    
    -- Make sure we are not removing data needed by analysis task
    DECLARE @@minID BIGINT, @@maxID BIGINT, @@lastEndTime DATETIME
    
    SELECT TOP 1 @@minID = MinRecordID, @@maxID = MaxRecordID, @@lastEndTime = LastEndTime 
    FROM [dbo].[bam_Metadata_AnalysisTasks] tasks
        LEFT JOIN [dbo].[bam_Metadata_Cubes] cubes ON tasks.CubeName = cubes.CubeName
        LEFT JOIN [dbo].[bam_Metadata_ActivityViews] activityViews ON (cubes.ViewName = activityViews.ViewName AND cubes.ActivityName = activityViews.ActivityName)
    WHERE activityViews.ActivityName = @activityName
    ORDER BY LastStartTime DESC
    
    IF (@@ERROR <> 0)
    BEGIN
        RAISERROR (N'BeginArchiving_FailToGetMinAnalysisRecordID', 16, 1)
        IF (@@TRANCOUNT > 0) ROLLBACK
        RETURN 255
    END
    
    -- Get analysis task's watermark
    DECLARE @@analysisRecordWatermark BIGINT
    IF @@lastEndTime IS NULL
        SET @@analysisRecordWatermark = @@minID
    ELSE
        SET @@analysisRecordWatermark = @@maxID

    -- Set qualified partitions' Archiving bit to 1
    UPDATE [dbo].[bam_Metadata_Partitions]
    SET ArchivingInProgress = 1
    WHERE ActivityName = @activityName    
        AND ArchivedTime IS NULL                -- hasn't been archived
        AND CreationTime < @@OnlineThreshold -- falling out of online window        
        AND (@@analysisRecordWatermark IS NULL
            OR MaxRecordID <= @@analysisRecordWatermark)
    
    -- Return immediately if no partitions qualifying for archiving
    IF (@@ROWCOUNT = 0)
    BEGIN
        COMMIT TRAN
        RETURN 0
    END

    -- Recreate archiving instance and relationship views
    EXEC [dbo].[bam_Metadata_RegenerateViews] @activityName, 1, @@OnlineThreshold, @@analysisRecordWatermark

    COMMIT TRAN    
    RETURN