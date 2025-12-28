CREATE TABLE [dbo].[bam_Metadata_Partitions] (
    [ActivityName]        [sysname] NOT NULL,
    [InstancesTable]      [sysname] NOT NULL,
    [RelationshipsTable]  [sysname] NOT NULL,
    [CreationTime]        DATETIME  NULL,
    [MinRecordID]         BIGINT    NULL,
    [MaxRecordID]         BIGINT    NULL,
    [ArchivingInProgress] BIT       DEFAULT ((0)) NULL,
    [ArchivedTime]        DATETIME  DEFAULT (NULL) NULL,
    FOREIGN KEY ([ActivityName]) REFERENCES [dbo].[bam_Metadata_Activities] ([ActivityName]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_ActivityAndInstanceTableName]
    ON [dbo].[bam_Metadata_Partitions]([ActivityName] ASC, [InstancesTable] ASC);

