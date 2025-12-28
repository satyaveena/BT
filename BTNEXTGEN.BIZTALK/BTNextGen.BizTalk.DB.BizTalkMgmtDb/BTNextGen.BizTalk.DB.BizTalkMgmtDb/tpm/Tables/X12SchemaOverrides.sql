CREATE TABLE [tpm].[X12SchemaOverrides] (
    [OverridesId]         INT            IDENTITY (1, 1) NOT NULL,
    [SettingsId]          INT            NOT NULL,
    [MessageId]           NVARCHAR (5)   NOT NULL,
    [SenderApplicationId] NVARCHAR (15)  NULL,
    [GSTargetNamespace]   NVARCHAR (230) NULL,
    [version]             ROWVERSION     NOT NULL,
    CONSTRAINT [PK_X12SchemasOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_X12SchemaOverrides_X12ProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[X12ProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_X12SchemaOverrides] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC, [SenderApplicationId] ASC)
);

