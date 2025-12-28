CREATE PROCEDURE [dbo].[bam_Metadata_BeginAnalysis]
(
    @cubeName NVARCHAR(256),            
    @cubeLastProcessedTime DATETIME        -- this is UTC time
)
AS
    BEGIN TRAN

    -- Pause the primary import until this SP is executed
    DECLARE @@activityName NVARCHAR(256), @@activityViewName NVARCHAR(256), @@viewName NVARCHAR(256)
    SELECT @@activityName = activities.ActivityName, @@activityViewName = activityViews.ActivityViewName, @@viewName = activityViews.ViewName
    FROM [dbo].[bam_Metadata_Activities] activities WITH (ROWLOCK, XLOCK)
        LEFT JOIN [dbo].[bam_Metadata_ActivityViews] activityViews ON activities.ActivityName = activityViews.ActivityName
        LEFT JOIN [dbo].[bam_Metadata_Cubes] cubes ON (activityViews.ViewName = cubes.ViewName AND activityViews.ActivityName = cubes.ActivityName)
    WHERE cubes.CubeName = @cubeName

    -- get the last execution data for this Analysis Task
    DECLARE @@isFirstRun BIT
    DECLARE @@LastMaxRecordID BIGINT
    DECLARE @@LastMinRecordID BIGINT
    DECLARE @@FailToUpdateEndTime BIT
    DECLARE @@LastStartTime DATETIME
    
    -- Get last run status
    SELECT TOP 1
        @@LastStartTime = LastStartTime,
        @@LastMaxRecordID = MaxRecordID,
        @@LastMinRecordID = MinRecordID,
        @@FailToUpdateEndTime = 
            CASE 
                WHEN LastEndTime IS NULL THEN 1
                ELSE 0
            END
    FROM [dbo].[bam_Metadata_AnalysisTasks]
    WHERE CubeName = @cubeName
    ORDER BY LastStartTime DESC
    
    IF @@LastStartTime IS NULL
        SET @@isFirstRun = 1
    ELSE
        SET @@isFirstRun = 0

    -- Note: VBScript cannot preserver the millisecond part of time, 
    -- truncate millisecond from current time first
    DECLARE @currTime DATETIME
    SET @currTime = GETUTCDATE()
    SET @currTime = DATEADD(ms, -DATEPART (ms, @currTime), @currTime)

    -- If the last run failed to update the end time at metadata table
    IF (@@isFirstRun = 0 AND @@FailToUpdateEndTime = 1) 
    BEGIN
        -- Cube process actually succeeded, just failed at last step updating the end time metadata
        -- Set end time to current time (this is the closest time we can get)
        IF (@@LastStartTime = @cubeLastProcessedTime)
            UPDATE [dbo].[bam_Metadata_AnalysisTasks] 
            SET LastEndTime = @currTime
            FROM [dbo].[bam_Metadata_AnalysisTasks] tasks, [dbo].[bam_Metadata_Cubes] cubes, [dbo].[bam_Metadata_ActivityViews] activityViews
            WHERE tasks.CubeName = @cubeName AND tasks.LastStartTime = @@LastStartTime 
                AND tasks.CubeName = cubes.CubeName 
                AND cubes.ViewName = activityViews.ViewName AND cubes.ActivityName = activityViews.ActivityName
                AND activityViews.ActivityName = @@activityName
        ELSE    -- Cube process failed, need to retry the last run
        BEGIN
            COMMIT TRAN
            RETURN
        END
        
    END
    -- else: either first run ever or everything succeeded in the last run
    
    -- Get a snapshot of the runing instances
    DECLARE @@running_table_name sysname
    SET @@running_table_name = 'bam_' + @cubeName + '_ActiveInstancesSnapshot'

    -- Re-populate the running snapshot table
    EXEC ('TRUNCATE TABLE dbo.[' + @@running_table_name + ']')
    EXEC ('INSERT INTO dbo.[' + @@running_table_name + '] SELECT * FROM dbo.[bam_' + @@viewName + '_' + @@activityViewName + '_ActiveAliasView]')

    -- get completed instances' Max and Min RecordID
    DECLARE @@MaxRecordID BIGINT
    DECLARE @@MinRecordID BIGINT
    CREATE TABLE #TempRecordID (MinRecordID BIGINT, MaxRecordID BIGINT)
    EXEC ('INSERT #TempRecordID(MinRecordID, MaxRecordID) SELECT MIN(RecordID), MAX(RecordID) FROM [bam_' + @@activityName + '_CompletedInstances]')
    SELECT TOP 1 @@MinRecordID = MinRecordID, @@MaxRecordID = MaxRecordID FROM #TempRecordID

    -- Calculate the MIN and MAX window of completed instances to process
    DECLARE @@WindowMax BIGINT
    DECLARE @@WindowMin BIGINT
    
    IF @@MaxRecordID IS NULL SET @@WindowMax = 0
    ELSE SET @@WindowMax = @@MaxRecordID

    
    IF @@isFirstRun = 1        -- this is the first time the task is run, get all the completed instances
        BEGIN
            IF @@MinRecordID IS NULL SET @@WindowMin = 0
            ELSE SET @@WindowMin = @@MinRecordID    

            UPDATE [dbo].[bam_Metadata_AnalysisTasks] 
            SET LastStartTime = @currTime, MinRecordID = @@WindowMin, MaxRecordID = @@WindowMax
            WHERE CubeName = @cubeName AND LastStartTime IS NULL
        END
    ELSE        -- the task was run before
        BEGIN
            IF @@WindowMax = 0 SET @@WindowMin = 0    -- There're no completed instances
            ELSE SET @@WindowMin = @@LastMaxRecordID + 1
            
            -- Insert a new record for current analysis DTS    run
            INSERT [dbo].[bam_Metadata_AnalysisTasks] (CubeName, MinRecordID, MaxRecordID, LastStartTime, LastEndTime)
            VALUES(@cubeName, @@WindowMin, @@WindowMax, @currTime, NULL)
        END
    
    -- Reset the completed instance window for analysis task
    DECLARE @@windowFilter sysname
    SET @@windowFilter = ' WHERE (RecordID IS NOT NULL) AND RecordID BETWEEN ' + STR(@@WindowMin) + ' AND ' + STR(@@WindowMax)

    DECLARE @@completed_view_name sysname
    SET @@completed_view_name = 'bam_' + @cubeName + '_CompletedInstancesWindow'
    EXEC ('ALTER VIEW dbo.[' + @@completed_view_name + '] AS SELECT * FROM dbo.[bam_' + @@viewName + '_' + @@activityViewName + '_CompletedAliasView] ' + @@windowFilter)

    COMMIT TRAN