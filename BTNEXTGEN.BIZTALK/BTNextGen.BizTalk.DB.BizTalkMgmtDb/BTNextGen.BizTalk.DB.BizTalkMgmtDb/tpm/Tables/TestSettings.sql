CREATE TABLE [tpm].[TestSettings] (
    [Id]             INT           NOT NULL,
    [SendSetting]    NVARCHAR (50) NOT NULL,
    [ReceiveSetting] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TestSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TestSettings_ProtocolSettings] FOREIGN KEY ([Id]) REFERENCES [tpm].[ProtocolSettings] ([ProtocolSettingsId]) ON DELETE CASCADE
);

