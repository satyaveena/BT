CREATE TABLE [dbo].[bam_BatchingActivity_Completed] (
    [RecordID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]                NVARCHAR (128) NOT NULL,
    [BatchStatus]               INT            NULL,
    [DestinationPartyID]        NVARCHAR (40)  NULL,
    [DestinationPartyName]      NVARCHAR (256) NULL,
    [ActivationTime]            DATETIME       NULL,
    [BatchOccurrenceCount]      INT            NULL,
    [EdiEncodingType]           INT            NULL,
    [BatchType]                 INT            NULL,
    [TargetedBatchCount]        NVARCHAR (32)  NULL,
    [ScheduledReleaseTime]      DATETIME       NULL,
    [BatchElementCount]         INT            NULL,
    [RejectedBatchElementCount] INT            NULL,
    [BatchSize]                 INT            NULL,
    [LastBatchAction]           INT            NULL,
    [CreationTime]              DATETIME       NULL,
    [ReleaseTime]               DATETIME       NULL,
    [BatchReleaseType]          INT            NULL,
    [BatchServiceID]            NVARCHAR (40)  NULL,
    [ActivationMessageID]       NVARCHAR (40)  NULL,
    [ReleaseMessageID]          NVARCHAR (40)  NULL,
    [TimeCreated]               DATETIME       NULL,
    [RowFlags]                  INT            NULL,
    [BatchCorrelationID]        NVARCHAR (40)  NULL,
    [BatchName]                 NVARCHAR (35)  NULL,
    [BatchID]                   NVARCHAR (35)  NULL,
    [AgreementName]             NVARCHAR (256) NULL,
    [LastModified]              DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_BatchingActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_BatchingActivity_Completed]([LastModified] ASC);

