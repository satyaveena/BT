CREATE TABLE [tpm].[BatchDescription] (
    [Id]                   BIGINT           IDENTITY (1, 1) NOT NULL,
    [OnewayAgreementId]    INT              NOT NULL,
    [Name]                 NVARCHAR (256)   NOT NULL,
    [Description]          NVARCHAR (256)   NULL,
    [Protocol]             NVARCHAR (50)    NOT NULL,
    [StartDate]            DATETIME2 (7)    NOT NULL,
    [EndDate]              DATETIME2 (7)    NOT NULL,
    [TerminationCount]     INT              NOT NULL,
    [FilterBytes]          VARBINARY (4000) NULL,
    [ReleaseCriteriaType]  SMALLINT         NOT NULL,
    [MessageCount]         INT              NOT NULL,
    [MessageScope]         SMALLINT         NULL,
    [InterchangeSize]      BIGINT           NOT NULL,
    [RecurrenceType]       SMALLINT         NOT NULL,
    [FirstRelease]         DATETIME2 (7)    NOT NULL,
    [RecurrenceTicks]      BIGINT           NOT NULL,
    [WeekDays]             INT              NOT NULL,
    [SendEmptyBatchSignal] BIT              NOT NULL,
    [Modified]             BIT              NOT NULL,
    [version]              ROWVERSION       NOT NULL,
    CONSTRAINT [PK_BatchDescription] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BatchDescription_OnewayAgreement] FOREIGN KEY ([OnewayAgreementId]) REFERENCES [tpm].[OnewayAgreement] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UK_BatchDescriptionName] UNIQUE NONCLUSTERED ([OnewayAgreementId] ASC, [Name] ASC)
);


GO
CREATE TRIGGER [tpm].[BatchDescriptionDeleteTrigger] ON [tpm].[BatchDescription]
FOR DELETE AS
    SET NOCOUNT ON
    DECLARE @batchName varchar(256)
    
    select @batchName = (select TOP 1 [Name]
                         from deleted
                         inner join [dbo].[PAM_Batching_Log] AS br
                         on deleted.[Id] = br.[BatchId]
                         where br.[BatchOrchestrationId] IS NOT NULL)
    
    IF @batchName IS NOT NULL 
    BEGIN
        ROLLBACK TRANSACTION
        RAISERROR(90001, 16, 1, @batchName)
    END
