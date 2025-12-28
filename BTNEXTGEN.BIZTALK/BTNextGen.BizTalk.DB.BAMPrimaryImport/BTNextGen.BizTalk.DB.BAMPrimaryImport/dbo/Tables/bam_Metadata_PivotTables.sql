CREATE TABLE [dbo].[bam_Metadata_PivotTables] (
    [CubeName]        [sysname]      NOT NULL,
    [CubeRef]         [sysname]      NOT NULL,
    [RtaName]         NVARCHAR (128) DEFAULT (NULL) NULL,
    [RTARef]          NVARCHAR (128) DEFAULT (NULL) NULL,
    [PivotTableName]  [sysname]      NOT NULL,
    [PivotTableXml]   NTEXT          NULL,
    [DimNamesUpdated] BIT            DEFAULT ((0)) NOT NULL,
    FOREIGN KEY ([CubeName]) REFERENCES [dbo].[bam_Metadata_Cubes] ([CubeName]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_PivotTableAndCubeName]
    ON [dbo].[bam_Metadata_PivotTables]([CubeName] ASC, [PivotTableName] ASC);

