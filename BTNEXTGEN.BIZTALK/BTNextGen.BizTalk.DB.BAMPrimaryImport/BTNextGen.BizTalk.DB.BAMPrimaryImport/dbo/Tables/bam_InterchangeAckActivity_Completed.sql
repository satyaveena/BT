CREATE TABLE [dbo].[bam_InterchangeAckActivity_Completed] (
    [RecordID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]           NVARCHAR (128) NOT NULL,
    [InterchangeControlNo] NVARCHAR (14)  NULL,
    [ReceiverID]           NVARCHAR (35)  NULL,
    [SenderID]             NVARCHAR (35)  NULL,
    [ReceiverQ]            NVARCHAR (4)   NULL,
    [SenderQ]              NVARCHAR (4)   NULL,
    [InterchangeDateTime]  DATETIME       NULL,
    [Direction]            INT            NULL,
    [AckProcessingStatus]  INT            NULL,
    [AckStatusCode]        INT            NULL,
    [AckIcn]               NVARCHAR (14)  NULL,
    [AckIcnDate]           NVARCHAR (6)   NULL,
    [AckIcnTime]           NVARCHAR (4)   NULL,
    [AckNoteCode1]         INT            NULL,
    [AckNoteCode2]         INT            NULL,
    [TimeCreated]          DATETIME       NULL,
    [RowFlags]             INT            NULL,
    [AckCorrelationId]     NVARCHAR (32)  NULL,
    [AgreementName]        NVARCHAR (256) NULL,
    [LastModified]         DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_InterchangeAckActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_InterchangeAckActivity_Completed]([LastModified] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_IAAIndex]
    ON [dbo].[bam_InterchangeAckActivity_Completed]([InterchangeControlNo] ASC, [ReceiverID] ASC, [SenderID] ASC, [ReceiverQ] ASC, [SenderQ] ASC, [InterchangeDateTime] ASC, [Direction] ASC);

