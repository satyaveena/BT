CREATE TABLE [dbo].[bam_ResendJournalActivity_Completed] (
    [RecordID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]         NVARCHAR (128) NOT NULL,
    [TrackingActivityID] NVARCHAR (255) NULL,
    [ResendIndex]        INT            NULL,
    [ResendStatus]       NVARCHAR (50)  NULL,
    [BTSInterchangeID]   NVARCHAR (128) NULL,
    [LastModified]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_ResendJournalActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_ResendJournalActivity_Completed]([LastModified] ASC);

