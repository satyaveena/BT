CREATE TABLE [dbo].[bam_FunctionalGroupInfo_Completed] (
    [RecordID]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]            NVARCHAR (128) NOT NULL,
    [InterchangeActivityID] NVARCHAR (128) NULL,
    [GroupControlNo]        NVARCHAR (14)  NULL,
    [FunctionalIDCode]      NVARCHAR (6)   NULL,
    [TSCount]               INT            NULL,
    [LastModified]          DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_FunctionalGroupInfo_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_FunctionalGroupInfo_Completed]([LastModified] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_FGIIndex]
    ON [dbo].[bam_FunctionalGroupInfo_Completed]([InterchangeActivityID] ASC, [GroupControlNo] ASC);

