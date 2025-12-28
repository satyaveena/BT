CREATE PROCEDURE [dbo].[bam_Metadata_GetPivotTableXml]
(
    @pivotTableName sysname,
    @cubeName sysname
)
AS
    SELECT PivotTableXml
    FROM [dbo].[bam_Metadata_PivotTables]
    WHERE PivotTableName = @pivotTableName AND CubeName = @cubeName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetPivotTableXml] TO [BAM_ManagementWS]
    AS [dbo];

