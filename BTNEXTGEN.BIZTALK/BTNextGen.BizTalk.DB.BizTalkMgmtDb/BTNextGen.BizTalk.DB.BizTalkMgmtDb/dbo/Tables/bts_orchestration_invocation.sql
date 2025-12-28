CREATE TABLE [dbo].[bts_orchestration_invocation] (
    [nID]                     INT     IDENTITY (1, 1) NOT NULL,
    [nOrchestrationID]        INT     NOT NULL,
    [nInvokedOrchestrationID] INT     NOT NULL,
    [nInvokeType]             TINYINT NOT NULL,
    CONSTRAINT [PK_bts_orchestration_invocation] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_orchestration_invocation_bts_orchestration] FOREIGN KEY ([nOrchestrationID]) REFERENCES [dbo].[bts_orchestration] ([nID]) ON DELETE CASCADE
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_invocation] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_invocation] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_invocation] TO [BTS_OPERATORS]
    AS [dbo];

