CREATE PROCEDURE [dbo].[bam_Metadata_GetRtaNames]
(
    @viewName       sysname,
    @activityName   sysname
)
AS
    SELECT rta.RtaName
    FROM bam_Metadata_Cubes cubes
        LEFT JOIN bam_Metadata_RealTimeAggregations rta ON cubes.CubeName = rta.CubeName
    WHERE cubes.ViewName = @viewName AND cubes.ActivityName = @activityName