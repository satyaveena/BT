CREATE TABLE [tpm].[EDIFACTEnvelopeOverrides] (
    [OverridesId]                    INT              IDENTITY (1, 1) NOT NULL,
    [SettingsId]                     INT              NOT NULL,
    [MessageId]                      NVARCHAR (15)    NOT NULL,
    [MessageVersion]                 NVARCHAR (6)     NOT NULL,
    [MessageRelease]                 NVARCHAR (6)     NOT NULL,
    [MessageAssociationAssignedCode] NVARCHAR (6)     NULL,
    [TargetNamespace]                NVARCHAR (230)   NOT NULL,
    [FunctionalGroupId]              NVARCHAR (6)     NULL,
    [SenderApplicationQualifier]     NVARCHAR (15)    NULL,
    [SenderApplicationId]            NVARCHAR (35)    NULL,
    [ReceiverApplicationQualifier]   NVARCHAR (15)    NULL,
    [ReceiverApplicationId]          NVARCHAR (35)    NULL,
    [ControllingAgencyCode]          NVARCHAR (6)     NULL,
    [GroupHeaderMessageVersion]      NVARCHAR (6)     NULL,
    [GroupHeaderMessageRelease]      NVARCHAR (6)     NULL,
    [AssociationAssignedCode]        NVARCHAR (6)     NULL,
    [SSOIdentifier]                  UNIQUEIDENTIFIER NULL,
    [version]                        ROWVERSION       NOT NULL,
    CONSTRAINT [PK_EDIFACTEnvelopeOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_EDIFACTEnvelopeOverrides_EDIFACTProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[EDIFACTProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_EDIFACTEnvelopeOverrides] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC, [MessageVersion] ASC, [MessageRelease] ASC, [MessageAssociationAssignedCode] ASC, [TargetNamespace] ASC)
);

