CREATE TABLE [dbo].[bts_orchestration_port] (
    [nID]              INT              IDENTITY (1, 1) NOT NULL,
    [uidGUID]          UNIQUEIDENTIFIER NULL,
    [nOrchestrationID] INT              NOT NULL,
    [nPortTypeID]      INT              NOT NULL,
    [nvcName]          NVARCHAR (256)   NOT NULL,
    [nPolarity]        INT              NOT NULL,
    [nBindingOption]   INT              NOT NULL,
    [nRolePortTypeID]  INT              NULL,
    [bLink]            BIT              NOT NULL,
    CONSTRAINT [PK_bts_orchestration_port] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_orchestration_port_bts_orchestration] FOREIGN KEY ([nOrchestrationID]) REFERENCES [dbo].[bts_orchestration] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [FK_bts_orchestration_port_bts_porttype] FOREIGN KEY ([nPortTypeID]) REFERENCES [dbo].[bts_porttype] ([nID]),
    CONSTRAINT [IX_bts_orchestration_port] UNIQUE NONCLUSTERED ([uidGUID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port] TO [BTS_OPERATORS]
    AS [dbo];

