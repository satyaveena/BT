CREATE TABLE [dbo].[bts_operation_msgtype] (
    [nID]            INT IDENTITY (1, 1) NOT NULL,
    [nOperationID]   INT NOT NULL,
    [nMessageTypeID] INT NOT NULL,
    [nType]          INT NOT NULL,
    CONSTRAINT [PK_bts_operation_msgtype] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_operation_msgtype_bts_porttype_operation] FOREIGN KEY ([nOperationID]) REFERENCES [dbo].[bts_porttype_operation] ([nID]) ON DELETE CASCADE
);

