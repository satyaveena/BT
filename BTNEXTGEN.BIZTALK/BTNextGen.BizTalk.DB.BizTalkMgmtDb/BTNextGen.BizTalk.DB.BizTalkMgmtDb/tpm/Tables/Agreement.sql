CREATE TABLE [tpm].[Agreement] (
    [Id]                         INT            IDENTITY (1, 1) NOT NULL,
    [PartnershipId]              INT            NOT NULL,
    [Name]                       NVARCHAR (256) NOT NULL,
    [Description]                NVARCHAR (MAX) NULL,
    [Enabled]                    BIT            NOT NULL,
    [StartDate]                  DATETIME2 (7)  NOT NULL,
    [EndDate]                    DATETIME2 (7)  NOT NULL,
    [Protocol]                   NVARCHAR (256) NOT NULL,
    [SenderProfileId]            INT            NOT NULL,
    [SenderProtocolSettingsId]   INT            NULL,
    [SenderOnewayAgreementId]    INT            NOT NULL,
    [ReceiverProfileId]          INT            NOT NULL,
    [ReceiverProtocolSettingsId] INT            NULL,
    [ReceiverOnewayAgreementId]  INT            NOT NULL,
    [CustomSettingsId]           INT            NULL,
    [version]                    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Agreements] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Agreement_Partnership] FOREIGN KEY ([PartnershipId]) REFERENCES [tpm].[Partnership] ([PartnershipId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Agreement_ReceiverProtocolSettings] FOREIGN KEY ([ReceiverProtocolSettingsId]) REFERENCES [tpm].[ProtocolSettings] ([ProtocolSettingsId]),
    CONSTRAINT [FK_Agreement_SenderProtocolSettings] FOREIGN KEY ([SenderProtocolSettingsId]) REFERENCES [tpm].[ProtocolSettings] ([ProtocolSettingsId]),
    CONSTRAINT [UK_AgreementName] UNIQUE NONCLUSTERED ([PartnershipId] ASC, [Name] ASC)
);


GO
CREATE TRIGGER [tpm].[AgreementDeleteTrigger] ON [tpm].[Agreement] 
FOR DELETE AS
    SET NOCOUNT ON
    DELETE [OnewayAgreement] FROM deleted, [OnewayAgreement] 
    WHERE ((deleted.[SenderOnewayAgreementId] = [OnewayAgreement].[Id]) 
            OR (deleted.[ReceiverOnewayAgreementId] = [OnewayAgreement].[Id]))
   
    DELETE [CustomSettings] FROM deleted, [CustomSettings]
    WHERE (deleted.[CustomSettingsId] = [CustomSettings].[Id])
