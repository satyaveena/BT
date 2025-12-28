CREATE TABLE [dbo].[bam_Metadata_AnalysisTasks] (
    [CubeName]      [sysname] NOT NULL,
    [MinRecordID]   BIGINT    NULL,
    [MaxRecordID]   BIGINT    NULL,
    [LastStartTime] DATETIME  DEFAULT (NULL) NULL,
    [LastEndTime]   DATETIME  DEFAULT (NULL) NULL,
    FOREIGN KEY ([CubeName]) REFERENCES [dbo].[bam_Metadata_Cubes] ([CubeName]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_CubeNameAndLastStartTime]
    ON [dbo].[bam_Metadata_AnalysisTasks]([CubeName] ASC, [LastStartTime] ASC);

