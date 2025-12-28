CREATE PROCEDURE [dbo].[bam_Metadata_SetRTAWindow]
(
    @viewName       sysname,
    @activityName   sysname,
    @rtaName        sysname,
    @rtaWindow      INT
)
AS
    UPDATE rta
    SET RTAWindow = @rtaWindow
    FROM bam_Metadata_ActivityViews activityViews
        JOIN bam_Metadata_Cubes cubes ON (activityViews.ViewName = cubes.ViewName AND activityViews.ActivityName = cubes.ActivityName)
        LEFT JOIN bam_Metadata_RealTimeAggregations rta ON cubes.CubeName = rta.CubeName
    WHERE activityViews.ViewName = @viewName AND activityViews.ActivityName = @activityName AND rta.RtaName = @rtaName
    
    IF @@ROWCOUNT = 0    
    BEGIN
        RAISERROR (N'SetRTAWindow_RTANotExist', 16, 1)
        RETURN
    END