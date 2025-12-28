CREATE TABLE [dbo].[bts_orchestration] (
    [nID]                  INT              IDENTITY (1, 1) NOT NULL,
    [uidGUID]              UNIQUEIDENTIFIER NULL,
    [uidOrchestrationType] UNIQUEIDENTIFIER NULL,
    [nAssemblyID]          INT              NOT NULL,
    [nItemID]              INT              NULL,
    [nvcNamespace]         NVARCHAR (256)   NULL,
    [nvcName]              NVARCHAR (256)   NOT NULL,
    [nvcFullName]          AS               (case when [nvcNamespace] IS NULL then [nvcName] else ([nvcNamespace]+N'.')+[nvcName] end),
    [nOrchestrationInfo]   INT              CONSTRAINT [DF_bts_service_nOrchestrationInfo] DEFAULT ((0)) NOT NULL,
    [nOrchestrationStatus] INT              CONSTRAINT [DF_bts_service_nOrchestrationStatus] DEFAULT ((0)) NOT NULL,
    [nAdminHostID]         INT              NULL,
    [dtModified]           DATETIME         NOT NULL,
    [nvcDescription]       NVARCHAR (1024)  NULL,
    CONSTRAINT [PK_bts_orchestration] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [fk_bts_orchestration_bts_item] FOREIGN KEY ([nItemID]) REFERENCES [dbo].[bts_item] ([id]),
    CONSTRAINT [IX_bts_orchestration_GUID] UNIQUE NONCLUSTERED ([uidGUID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_orchestration] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration] TO [BTS_OPERATORS]
    AS [dbo];

