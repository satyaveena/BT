CREATE TABLE [dbo].[bam_FunctionalAckActivity_Completed] (
    [RecordID]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]            NVARCHAR (128) NOT NULL,
    [InterchangeActivityID] NVARCHAR (128) NULL,
    [GroupControlNo]        NVARCHAR (14)  NULL,
    [InterchangeControlNo]  NVARCHAR (14)  NULL,
    [ReceiverID]            NVARCHAR (35)  NULL,
    [SenderID]              NVARCHAR (35)  NULL,
    [ReceiverQ]             NVARCHAR (4)   NULL,
    [SenderQ]               NVARCHAR (4)   NULL,
    [InterchangeDateTime]   DATETIME       NULL,
    [Direction]             INT            NULL,
    [AckProcessingStatus]   INT            NULL,
    [AckStatusCode]         INT            NULL,
    [DeliveredTSCount]      INT            NULL,
    [AcceptedTSCount]       INT            NULL,
    [AckIcn]                NVARCHAR (14)  NULL,
    [AckIcnDate]            NVARCHAR (6)   NULL,
    [AckIcnTime]            NVARCHAR (4)   NULL,
    [ErrorCode1]            INT            NULL,
    [ErrorCode2]            INT            NULL,
    [ErrorCode3]            INT            NULL,
    [ErrorCode4]            INT            NULL,
    [ErrorCode5]            INT            NULL,
    [TimeCreated]           DATETIME       NULL,
    [RowFlags]              INT            NULL,
    [AgreementName]         NVARCHAR (256) NULL,
    [LastModified]          DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_FunctionalAckActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_FunctionalAckActivity_Completed]([LastModified] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_FAAIndex]
    ON [dbo].[bam_FunctionalAckActivity_Completed]([GroupControlNo] ASC, [ReceiverID] ASC, [SenderID] ASC, [ReceiverQ] ASC, [SenderQ] ASC, [Direction] ASC);

