CREATE TABLE [tpm].[BusinessProfile] (
    [ProfileId]        INT            IDENTITY (1, 1) NOT NULL,
    [PartnerId]        INT            NOT NULL,
    [Name]             NVARCHAR (256) NOT NULL,
    [Description]      NVARCHAR (256) NULL,
    [CustomSettingsId] INT            NULL,
    [version]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_BusinessProfile] PRIMARY KEY CLUSTERED ([ProfileId] ASC),
    CONSTRAINT [FK_BusinessProfile_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [tpm].[Partner] ([PartnerId]),
    CONSTRAINT [UK_ProfileName] UNIQUE NONCLUSTERED ([PartnerId] ASC, [Name] ASC)
);


GO
CREATE TRIGGER [tpm].[BusinessProfileTrigger] ON [tpm].[BusinessProfile]
INSTEAD OF DELETE AS
    SET NOCOUNT ON
    DELETE [Agreement] FROM deleted, [Agreement] 
    WHERE ((deleted.[ProfileId] = [Agreement].[SenderProfileId]) 
            OR (deleted.[ProfileId] = [Agreement].[ReceiverProfileId]))
            
    DELETE [CustomSettings] FROM deleted, [CustomSettings]
    WHERE (deleted.[CustomSettingsId] = [CustomSettings].[Id])
    
    DELETE [BusinessProfile] FROM deleted, [BusinessProfile] 
    WHERE (deleted.[ProfileId] = [BusinessProfile].[ProfileId])
