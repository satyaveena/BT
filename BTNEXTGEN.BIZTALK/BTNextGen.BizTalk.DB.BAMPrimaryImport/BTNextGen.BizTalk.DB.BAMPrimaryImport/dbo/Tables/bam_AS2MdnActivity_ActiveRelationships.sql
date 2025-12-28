CREATE TABLE [dbo].[bam_AS2MdnActivity_ActiveRelationships] (
    [ActivityID]          NVARCHAR (128) NULL,
    [ReferenceName]       NVARCHAR (128) NULL,
    [ReferenceData]       NVARCHAR (128) NULL,
    [ReferenceType]       NVARCHAR (128) DEFAULT (N'BizTalkService') NOT NULL,
    [LongReferenceData]   NTEXT          NULL,
    [ReferenceDataExtend] NVARCHAR (896) NULL
);


GO
CREATE CLUSTERED INDEX [CI_ActivityIDandOtherActivityNameandID]
    ON [dbo].[bam_AS2MdnActivity_ActiveRelationships]([ActivityID] ASC, [ReferenceName] ASC, [ReferenceData] ASC);

