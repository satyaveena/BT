CREATE TABLE [dbo].[bam_BatchInterchangeActivity_Active] (
    [ActivityID]           NVARCHAR (128) NOT NULL,
    [IsVisible]            BIT            DEFAULT (NULL) NULL,
    [IsComplete]           BIT            DEFAULT (NULL) NULL,
    [InterchangeControlNo] NVARCHAR (14)  NULL,
    [ReceiverPartyName]    NVARCHAR (256) NULL,
    [SenderPartyName]      NVARCHAR (256) NULL,
    [ReceiverID]           NVARCHAR (35)  NULL,
    [SenderID]             NVARCHAR (35)  NULL,
    [ReceiverQ]            NVARCHAR (4)   NULL,
    [SenderQ]              NVARCHAR (4)   NULL,
    [InterchangeDateTime]  DATETIME       NULL,
    [Direction]            INT            NULL,
    [TimeCreated]          DATETIME       NULL,
    [RowFlags]             INT            NULL,
    [BatchCorrelationID]   NVARCHAR (40)  NULL,
    [AgreementName]        NVARCHAR (256) NULL,
    [LastModified]         DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

