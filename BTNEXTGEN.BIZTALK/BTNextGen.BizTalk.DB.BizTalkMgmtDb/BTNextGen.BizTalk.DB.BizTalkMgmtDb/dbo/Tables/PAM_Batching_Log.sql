CREATE TABLE [dbo].[PAM_Batching_Log] (
    [BatchOrchestrationId] UNIQUEIDENTIFIER NULL,
    [NumOccurences]        INT              NULL,
    [BatchId]              BIGINT           NOT NULL,
    CONSTRAINT [PK_PAM_Batching_Log] PRIMARY KEY CLUSTERED ([BatchId] ASC)
);

