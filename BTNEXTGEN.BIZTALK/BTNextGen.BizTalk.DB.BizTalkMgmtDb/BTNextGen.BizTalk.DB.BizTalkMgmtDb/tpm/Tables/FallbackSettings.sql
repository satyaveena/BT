CREATE TABLE [tpm].[FallbackSettings] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [SenderId]           INT           NOT NULL,
    [ReceiverId]         INT           NOT NULL,
    [ProtocolSettingsId] INT           NOT NULL,
    [ProtocolName]       NVARCHAR (50) NOT NULL,
    [Enabled]            BIT           NOT NULL,
    [CustomSettingsId]   INT           NULL,
    [version]            ROWVERSION    NOT NULL,
    CONSTRAINT [PK_FallbackSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FallbackSettings_ProtocolSettings] FOREIGN KEY ([ProtocolSettingsId]) REFERENCES [tpm].[ProtocolSettings] ([ProtocolSettingsId]),
    CONSTRAINT [FK_FallbackSettings_ReceiverIdentity] FOREIGN KEY ([ReceiverId]) REFERENCES [tpm].[BusinessIdentity] ([Id]),
    CONSTRAINT [FK_FallbackSettings_SenderIdentity] FOREIGN KEY ([SenderId]) REFERENCES [tpm].[BusinessIdentity] ([Id]),
    CONSTRAINT [UK_FallbackSettings] UNIQUE NONCLUSTERED ([ProtocolName] ASC)
);


GO
CREATE TRIGGER [tpm].[FallbackSettingsDeleteTrigger] ON [tpm].[FallbackSettings] 
FOR DELETE AS
    SET NOCOUNT ON
    DELETE [ProtocolSettings] FROM deleted, [ProtocolSettings] 
    WHERE (deleted.[ProtocolSettingsId] = [ProtocolSettings].[ProtocolSettingsId]) 
    
    DELETE [CustomSettings] FROM deleted, [CustomSettings]
    WHERE (deleted.[CustomSettingsId] = [CustomSettings].[Id])
    
    DELETE [BusinessIdentity] FROM deleted, [BusinessIdentity]
    WHERE (deleted.[SenderId] = [BusinessIdentity].[Id])
    DELETE [BusinessIdentity] FROM deleted, [BusinessIdentity]
    WHERE (deleted.[ReceiverId] = [BusinessIdentity].[Id])
