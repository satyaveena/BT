CREATE TABLE [tpm].[X12EnvelopesOverrides] (
    [OverridesId]              INT            IDENTITY (1, 1) NOT NULL,
    [SettingsId]               INT            NOT NULL,
    [TargetNamespace]          NVARCHAR (230) NULL,
    [ProtocolVersion]          NVARCHAR (5)   NULL,
    [MessageId]                NVARCHAR (5)   NOT NULL,
    [SenderApplicationId]      NVARCHAR (15)  NULL,
    [ReceiverApplicationId]    NVARCHAR (15)  NULL,
    [FunctionalIdentifierCode] NVARCHAR (15)  NULL,
    [DateFormat]               SMALLINT       NOT NULL,
    [TimeFormat]               SMALLINT       NOT NULL,
    [ResponsibleAgencyCode]    SMALLINT       NOT NULL,
    [HeaderVersion]            NVARCHAR (12)  NULL,
    [version]                  ROWVERSION     NOT NULL,
    CONSTRAINT [PK_X12EnvelopesOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_X12EnvelopesOverrrides_X12ProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[X12ProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_X12EnvelopesOverrides] UNIQUE NONCLUSTERED ([SettingsId] ASC, [TargetNamespace] ASC, [ProtocolVersion] ASC, [MessageId] ASC)
);

