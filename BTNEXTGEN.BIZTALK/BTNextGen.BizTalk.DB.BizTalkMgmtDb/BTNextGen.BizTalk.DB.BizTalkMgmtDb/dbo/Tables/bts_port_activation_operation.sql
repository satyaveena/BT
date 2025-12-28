CREATE TABLE [dbo].[bts_port_activation_operation] (
    [nID]              INT IDENTITY (1, 1) NOT NULL,
    [nOrchestrationID] INT NOT NULL,
    [nPortID]          INT NOT NULL,
    [nOperationID]     INT NULL,
    CONSTRAINT [PK_bts_port_activation_operation] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_port_activation_operation_bts_orchestration] FOREIGN KEY ([nOrchestrationID]) REFERENCES [dbo].[bts_orchestration] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [FK_bts_port_activation_operation_bts_porttype_operation] FOREIGN KEY ([nOperationID]) REFERENCES [dbo].[bts_porttype_operation] ([nID])
);

