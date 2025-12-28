CREATE TABLE [dbo].[bam_FirstDataRecon_CompletedRelationships] (
    [RecordID]            BIGINT         NULL,
    [ActivityID]          NVARCHAR (128) NULL,
    [ReferenceName]       NVARCHAR (128) NULL,
    [ReferenceData]       NVARCHAR (128) NULL,
    [ReferenceType]       NVARCHAR (128) DEFAULT (N'BizTalkService') NOT NULL,
    [LongReferenceData]   NTEXT          NULL,
    [ReferenceDataExtend] NVARCHAR (896) NULL
);


GO
CREATE CLUSTERED INDEX [CI_RecordIDandOtherActivityNameandID]
    ON [dbo].[bam_FirstDataRecon_CompletedRelationships]([RecordID] ASC, [ReferenceName] ASC, [ReferenceData] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_FirstDataRecon_CompletedRelationships]([ActivityID] ASC);

