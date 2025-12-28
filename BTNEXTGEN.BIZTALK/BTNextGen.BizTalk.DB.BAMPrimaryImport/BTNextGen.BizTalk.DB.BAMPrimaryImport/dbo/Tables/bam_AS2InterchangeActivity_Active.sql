CREATE TABLE [dbo].[bam_AS2InterchangeActivity_Active] (
    [ActivityID]           NVARCHAR (128)  NOT NULL,
    [IsVisible]            BIT             DEFAULT (NULL) NULL,
    [IsComplete]           BIT             DEFAULT (NULL) NULL,
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
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

