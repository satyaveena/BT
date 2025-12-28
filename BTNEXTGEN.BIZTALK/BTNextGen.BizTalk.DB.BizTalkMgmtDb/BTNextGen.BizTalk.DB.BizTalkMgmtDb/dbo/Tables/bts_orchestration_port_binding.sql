CREATE TABLE [dbo].[bts_orchestration_port_binding] (
    [nID]            INT      IDENTITY (1, 1) NOT NULL,
    [nOrcPortID]     INT      NOT NULL,
    [nReceivePortID] INT      NULL,
    [nSendPortID]    INT      NULL,
    [nSpgID]         INT      NULL,
    [DateModified]   DATETIME NOT NULL,
    CONSTRAINT [bts_orchestration_port_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_applicationbinding_bts_orchestration_port_binding_receiveport] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetOrchestrationPortAppId]([nOrcPortID]),[dbo].[adm_GetReceivePortAppId]([nReceivePortID]))=(1)),
    CONSTRAINT [CK_applicationbinding_bts_orchestration_port_binding_sendport] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetOrchestrationPortAppId]([nOrcPortID]),[dbo].[adm_GetSendPortAppId]([nSendPortID]))=(1)),
    CONSTRAINT [CK_applicationbinding_bts_orchestration_port_binding_sendportgroup] CHECK ([dbo].[adm_IsReferencedBy]([dbo].[adm_GetOrchestrationPortAppId]([nOrcPortID]),[dbo].[adm_GetSendPortGroupAppId]([nSpgID]))=(1)),
    CONSTRAINT [bts_orchestration_port_foreign_receiveportid] FOREIGN KEY ([nReceivePortID]) REFERENCES [dbo].[bts_receiveport] ([nID]),
    CONSTRAINT [bts_orchestration_port_foreign_sendportid] FOREIGN KEY ([nSendPortID]) REFERENCES [dbo].[bts_sendport] ([nID]),
    CONSTRAINT [bts_orchestration_port_foreign_spgid] FOREIGN KEY ([nSpgID]) REFERENCES [dbo].[bts_sendportgroup] ([nID]),
    CONSTRAINT [bts_orcport_binding_foreign_orcportid] FOREIGN KEY ([nOrcPortID]) REFERENCES [dbo].[bts_orchestration_port] ([nID]) ON DELETE CASCADE
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_orchestration_port_binding] TO [BTS_OPERATORS]
    AS [dbo];

