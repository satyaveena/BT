CREATE TABLE [dbo].[bam_AS2InterchangeActivity_Completed] (
    [RecordID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActivityID]           NVARCHAR (128)  NOT NULL,
    [InterchangeControlNo] NVARCHAR (14)   NULL,
    [ReceiverID]           NVARCHAR (35)   NULL,
    [SenderID]             NVARCHAR (35)   NULL,
    [ReceiverQ]            NVARCHAR (4)    NULL,
    [SenderQ]              NVARCHAR (4)    NULL,
    [InterchangeDateTime]  DATETIME        NULL,
    [Direction]            INT             NULL,
    [MessageID]            NVARCHAR (1000) NULL,
    [AS2From]              NVARCHAR (128)  NULL,
    [AS2To]                NVARCHAR (128)  NULL,
    [TimeCreated]          DATETIME        NULL,
    [RowFlags]             INT             NULL,
    [AgreementName]        NVARCHAR (256)  NULL,
    [LastModified]         DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_AS2InterchangeActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_AS2InterchangeActivity_Completed]([LastModified] ASC);

