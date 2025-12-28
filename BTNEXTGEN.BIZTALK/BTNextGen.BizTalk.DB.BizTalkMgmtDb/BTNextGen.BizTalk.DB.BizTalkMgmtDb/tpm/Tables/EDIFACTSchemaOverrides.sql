CREATE TABLE [tpm].[EDIFACTSchemaOverrides] (
    [OverridesId]                INT            IDENTITY (1, 1) NOT NULL,
    [SettingsId]                 INT            NOT NULL,
    [MessageId]                  NVARCHAR (15)  NULL,
    [MessageVersion]             NVARCHAR (6)   NOT NULL,
    [MessageRelease]             NVARCHAR (6)   NOT NULL,
    [ApplicationSenderID]        NVARCHAR (15)  NULL,
    [ApplicationSenderQualifier] NVARCHAR (15)  NULL,
    [AssociationAssignedCode]    NVARCHAR (6)   NULL,
    [TargetNamespace]            NVARCHAR (230) NOT NULL,
    [version]                    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EDIFACTSchemaOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_EDIFACTSchemaOverrides_EDIFACTProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[EDIFACTProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_EDIFACTSchemaOverrides] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC, [MessageVersion] ASC, [MessageRelease] ASC, [ApplicationSenderID] ASC, [ApplicationSenderQualifier] ASC, [AssociationAssignedCode] ASC)
);

