CREATE PROCEDURE [dbo].[bam_Metadata_UpdatePivotTable]
(
    @pivotTableName sysname,
    @cubeName sysname,
    @pivotTableXml NTEXT,
    @dimNamesUpdated BIT = 1
)
AS
    UPDATE [dbo].[bam_Metadata_PivotTables]
    SET PivotTableXml = @pivotTableXml,
        DimNamesUpdated = @dimNamesUpdated
    WHERE PivotTableName = @pivotTableName AND CubeName = @cubeName