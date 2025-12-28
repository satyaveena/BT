CREATE TABLE [dbo].[bam_Metadata_RealTimeAggregations] (
    [CubeName]          [sysname]       NOT NULL,
    [RtaName]           [sysname]       NOT NULL,
    [RTAWindow]         INT             NULL,
    [Timeslice]         INT             NULL,
    [LastRtaDataUpdate] DATETIME        NULL,
    [ConnectionString]  NVARCHAR (3600) NULL,
    FOREIGN KEY ([CubeName]) REFERENCES [dbo].[bam_Metadata_Cubes] ([CubeName]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_CubeAndRtaName]
    ON [dbo].[bam_Metadata_RealTimeAggregations]([CubeName] ASC, [RtaName] ASC);

