CREATE TABLE [tpm].[Partner] (
    [PartnerId]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (256) NOT NULL,
    [Description]      NVARCHAR (256) NULL,
    [SID]              NVARCHAR (256) NULL,
    [CertificateName]  NVARCHAR (MAX) NULL,
    [CertificateHash]  NVARCHAR (256) NULL,
    [CustomSettingsId] INT            NULL,
    [DateModified]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [CustomData]       NTEXT          NULL,
    [version]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Partner] PRIMARY KEY CLUSTERED ([PartnerId] ASC),
    CONSTRAINT [UK_PartnerName] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UK_CertificateHash_NonNull]
    ON [tpm].[Partner]([CertificateHash] ASC) WHERE ([CertificateHash] IS NOT NULL AND [CertificateHash]<>'');


GO
CREATE TRIGGER [tpm].[PartnerDeleteTrigger]
ON [tpm].[Partner] INSTEAD OF DELETE AS
    SET NOCOUNT ON
    DELETE [Partnership] FROM deleted, [Partnership] 
    WHERE ((deleted.[PartnerId] = [Partnership].[PartnerAId]) 
        OR (deleted.[PartnerId] = [Partnership].[PartnerBId]))
    DELETE [BusinessProfile] FROM deleted, [BusinessProfile] 
    WHERE (deleted.[PartnerId] = [BusinessProfile].[PartnerId]) 
    DELETE [CustomSettings] FROM deleted, [CustomSettings]
    WHERE (deleted.[CustomSettingsId] = [CustomSettings].[Id])
            
    DELETE [Partner] FROM deleted, [Partner] where [deleted].[PartnerId] = [Partner].[PartnerId]
