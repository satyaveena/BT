CREATE PROCEDURE [dbo].[bam_Metadata_GetPivotTablesForView]
(
    @viewName NVARCHAR(128) = NULL
)
AS
IF (@viewName IS NOT NULL)
    SELECT PivotTables.PivotTableName AS PivotTableName, PivotTables.CubeRef AS CubeRef, PivotTables.RTARef AS RTARef, PivotTables.PivotTableXml AS PivotTableXml
    FROM bam_Metadata_Views BamViews
        LEFT OUTER JOIN bam_Metadata_ActivityViews ActivityViews ON BamViews.ViewName = ActivityViews.ViewName
        LEFT OUTER JOIN bam_Metadata_Cubes Cubes ON (ActivityViews.ViewName = Cubes.ViewName AND ActivityViews.ActivityName = Cubes.ActivityName)
        LEFT OUTER JOIN bam_Metadata_PivotTables PivotTables ON Cubes.CubeName = PivotTables.CubeName
    WHERE PivotTables.CubeRef IS NOT NULL AND BamViews.ViewName = @viewName
ELSE
    SELECT PivotTables.PivotTableName AS PivotTableName, PivotTables.CubeRef AS CubeRef, PivotTables.RTARef AS RTARef, PivotTables.PivotTableXml AS PivotTableXml
    FROM bam_Metadata_PivotTables PivotTables
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetPivotTablesForView] TO [BAM_ManagementWS]
    AS [dbo];

