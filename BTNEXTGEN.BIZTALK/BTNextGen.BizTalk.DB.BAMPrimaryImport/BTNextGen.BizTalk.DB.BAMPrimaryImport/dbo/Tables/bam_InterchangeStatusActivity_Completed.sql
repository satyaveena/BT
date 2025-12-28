CREATE TABLE [dbo].[bam_InterchangeStatusActivity_Completed] (
    [RecordID]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]               NVARCHAR (128) NOT NULL,
    [InterchangeControlNo]     NVARCHAR (14)  NULL,
    [ReceiverID]               NVARCHAR (35)  NULL,
    [SenderID]                 NVARCHAR (35)  NULL,
    [ReceiverQ]                NVARCHAR (4)   NULL,
    [SenderQ]                  NVARCHAR (4)   NULL,
    [ReceiverPartyName]        NVARCHAR (256) NULL,
    [SenderPartyName]          NVARCHAR (256) NULL,
    [InterchangeDateTime]      DATETIME       NULL,
    [Direction]                INT            NULL,
    [AckStatusCode]            INT            NULL,
    [GroupCount]               INT            NULL,
    [EdiMessageType]           INT            NULL,
    [PortID]                   NVARCHAR (40)  NULL,
    [IsInterchangeAckExpected] INT            NULL,
    [IsFunctionalAckExpected]  INT            NULL,
    [TimeCreated]              DATETIME       NULL,
    [RowFlags]                 INT            NULL,
    [AckCorrelationId]         NVARCHAR (32)  NULL,
    [TsCorrelationId]          NVARCHAR (32)  NULL,
    [AgreementName]            NVARCHAR (256) NULL,
    [LastModified]             DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_InterchangeStatusActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_InterchangeStatusActivity_Completed]([LastModified] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_ISAIndex]
    ON [dbo].[bam_InterchangeStatusActivity_Completed]([InterchangeControlNo] ASC, [ReceiverID] ASC, [SenderID] ASC, [ReceiverQ] ASC, [SenderQ] ASC, [InterchangeDateTime] ASC, [Direction] ASC);

