CREATE TABLE [dbo].[bts_enlistedparty_operation_mapping] (
    [nID]              INT      IDENTITY (1, 1) NOT NULL,
    [nOperationID]     INT      NOT NULL,
    [nPartySendPortID] INT      NOT NULL,
    [nPortMappingID]   INT      NOT NULL,
    [DateModified]     DATETIME NOT NULL,
    CONSTRAINT [bts_enlistedparty_operation_mapping_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_enlistedparty_operation_mapping] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetRolelinkTypeNonSystemAppId]([nPortMappingID]),[dbo].[adm_GetPartySendPortAppId]([nPartySendPortID]))=(1)),
    CONSTRAINT [bts_enlistedparty_mapping_foreign_party_sendportid] FOREIGN KEY ([nPartySendPortID]) REFERENCES [tpm].[SendPortReference] ([Id]),
    CONSTRAINT [bts_enlistedparty_mapping_foreign_portmappingid] FOREIGN KEY ([nPortMappingID]) REFERENCES [dbo].[bts_enlistedparty_port_mapping] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_enlistedparty_operation_mapping_foreign_operationid] FOREIGN KEY ([nOperationID]) REFERENCES [dbo].[bts_porttype_operation] ([nID])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_enlistedparty_operation_mapping] TO [BTS_OPERATORS]
    AS [dbo];

