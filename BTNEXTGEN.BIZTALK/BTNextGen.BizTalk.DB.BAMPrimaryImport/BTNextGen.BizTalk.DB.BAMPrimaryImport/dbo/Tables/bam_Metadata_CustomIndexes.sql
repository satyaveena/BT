CREATE TABLE [dbo].[bam_Metadata_CustomIndexes] (
    [ActivityName] [sysname]       NOT NULL,
    [IndexName]    [sysname]       NOT NULL,
    [ColumnsList]  NVARCHAR (3000) NULL,
    FOREIGN KEY ([ActivityName]) REFERENCES [dbo].[bam_Metadata_Activities] ([ActivityName]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_ActivityAndIndexName]
    ON [dbo].[bam_Metadata_CustomIndexes]([ActivityName] ASC, [IndexName] ASC);

