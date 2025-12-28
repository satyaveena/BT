CREATE TABLE [dbo].[bam_TransactionSetActivity_Completed] (
    [RecordID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]              NVARCHAR (128) NOT NULL,
    [InterchangeControlNo]    NVARCHAR (14)  NULL,
    [ReceiverID]              NVARCHAR (35)  NULL,
    [SenderID]                NVARCHAR (35)  NULL,
    [ReceiverQ]               NVARCHAR (4)   NULL,
    [SenderQ]                 NVARCHAR (4)   NULL,
    [InterchangeDateTime]     DATETIME       NULL,
    [Direction]               INT            NULL,
    [ReceiverPartyName]       NVARCHAR (256) NULL,
    [SenderPartyName]         NVARCHAR (256) NULL,
    [ApplicationSender]       NVARCHAR (41)  NULL,
    [ApplicationReceiver]     NVARCHAR (41)  NULL,
    [GroupDateTime]           DATETIME       NULL,
    [GroupControlNo]          NVARCHAR (14)  NULL,
    [TransactionSetId]        NVARCHAR (6)   NULL,
    [DocType]                 NVARCHAR (256) NULL,
    [TransactionSetControlNo] NVARCHAR (14)  NULL,
    [AckStatusCode]           INT            NULL,
    [BatchProcessing]         INT            NULL,
    [ProcessingDateTime]      DATETIME       NULL,
    [GroupOrdinal]            INT            NULL,
    [TransactionSetOrdinal]   INT            NULL,
    [MessageContentKey]       NVARCHAR (40)  NULL,
    [TimeCreated]             DATETIME       NULL,
    [RowFlags]                INT            NULL,
    [TsCorrelationId]         NVARCHAR (32)  NULL,
    [AgreementName]           NVARCHAR (256) NULL,
    [LastModified]            DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_TransactionSetActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_TransactionSetActivity_Completed]([LastModified] ASC);

