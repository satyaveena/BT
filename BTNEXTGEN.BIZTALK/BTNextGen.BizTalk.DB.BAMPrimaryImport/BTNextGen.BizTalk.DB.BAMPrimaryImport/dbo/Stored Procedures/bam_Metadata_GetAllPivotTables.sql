CREATE PROCEDURE [dbo].[bam_Metadata_GetAllPivotTables]
AS
    SELECT CubeName, RtaName, PivotTableName, PivotTableXml, DimNamesUpdated
    FROM [dbo].[bam_Metadata_PivotTables]