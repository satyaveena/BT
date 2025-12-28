CREATE TABLE [tpm].[OnewayAgreement] (
    [Id]                 INT        IDENTITY (1, 1) NOT NULL,
    [ProtocolSettingsId] INT        NOT NULL,
    [SenderId]           INT        NOT NULL,
    [ReceiverId]         INT        NOT NULL,
    [version]            ROWVERSION NOT NULL,
    CONSTRAINT [PK_OnewayAgreement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OnewayAgreement_ProtocolSettings] FOREIGN KEY ([ProtocolSettingsId]) REFERENCES [tpm].[ProtocolSettings] ([ProtocolSettingsId]),
    CONSTRAINT [FK_OnewayAgreement_ReceiverIdentity] FOREIGN KEY ([ReceiverId]) REFERENCES [tpm].[BusinessIdentity] ([Id]),
    CONSTRAINT [FK_OnewayAgreement_SenderIdentity] FOREIGN KEY ([SenderId]) REFERENCES [tpm].[BusinessIdentity] ([Id])
);


GO
CREATE TRIGGER [tpm].[OnewayAgreementDeleteTrigger] ON [tpm].[OnewayAgreement] 
FOR DELETE AS
    SET NOCOUNT ON
    DELETE [ProtocolSettings] FROM deleted, [ProtocolSettings] 
    WHERE (deleted.[ProtocolSettingsId] = [ProtocolSettings].[ProtocolSettingsId]) 
            
