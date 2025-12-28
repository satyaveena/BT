CREATE PROCEDURE [dbo].[bam_Metadata_GetRTAWindow]
(
    @viewName       sysname,
    @activityName   sysname,
    @rtaName        sysname
)
AS
    SELECT rta.RTAWindow RTAWindow
    FROM bam_Metadata_ActivityViews activityViews
        JOIN bam_Metadata_Cubes cubes ON (activityViews.ViewName = cubes.ViewName AND activityViews.ActivityName = cubes.ActivityName)
        LEFT JOIN bam_Metadata_RealTimeAggregations rta ON cubes.CubeName = rta.CubeName
    WHERE activityViews.ViewName = @viewName AND activityViews.ActivityName = @activityName AND rta.RtaName = @rtaName