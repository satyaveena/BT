CREATE TABLE [tpm].[ProtocolSettings] (
    [ProtocolSettingsId] INT            IDENTITY (1, 1) NOT NULL,
    [ProfileId]          INT            NULL,
    [ProtocolName]       NVARCHAR (50)  NOT NULL,
    [SettingsName]       NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_ProtocolSettings] PRIMARY KEY CLUSTERED ([ProtocolSettingsId] ASC),
    CONSTRAINT [FK_ProtocolSettings_BusinessProfile] FOREIGN KEY ([ProfileId]) REFERENCES [tpm].[BusinessProfile] ([ProfileId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_Profile_SettingsName]
    ON [tpm].[ProtocolSettings]([ProfileId] ASC, [SettingsName] ASC) WHERE ([ProfileId] IS NOT NULL);

