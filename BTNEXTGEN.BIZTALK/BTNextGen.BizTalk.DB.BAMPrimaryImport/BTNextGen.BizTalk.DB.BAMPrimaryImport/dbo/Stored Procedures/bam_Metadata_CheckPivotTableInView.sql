CREATE PROCEDURE [dbo].[bam_Metadata_CheckPivotTableInView]
(
    @pivotTableName   NVARCHAR(128),
    @cubeName         NVARCHAR(128),
    @viewName         NVARCHAR(128)
)
AS
    SELECT count(*) 
    FROM bam_Metadata_PivotTables PivotTables
        JOIN bam_Metadata_Cubes Cubes ON PivotTables.CubeName = Cubes.CubeName
        JOIN bam_Metadata_ActivityViews ActivityView ON (Cubes.ViewName = ActivityView.ViewName AND Cubes.ActivityName = ActivityView.ActivityName)
    WHERE PivotTables.PivotTableName = @pivotTableName AND PivotTables.CubeName = @cubeName AND ActivityView.ViewName = @viewName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_CheckPivotTableInView] TO [BAM_ManagementWS]
    AS [dbo];

