CREATE TABLE [dbo].[bam_EmailTotals_ActiveInstancesSnapshot] (
    [RecordID]       INT            NULL,
    [ActivityID]     NVARCHAR (128) NOT NULL,
    [LastModified]   DATETIME       NULL,
    [EmailRecipient] NVARCHAR (256) NULL,
    [EmailSender]    NVARCHAR (256) NULL,
    [EmailSent]      DATETIME       NULL,
    [FileReceived]   DATETIME       NULL
);

