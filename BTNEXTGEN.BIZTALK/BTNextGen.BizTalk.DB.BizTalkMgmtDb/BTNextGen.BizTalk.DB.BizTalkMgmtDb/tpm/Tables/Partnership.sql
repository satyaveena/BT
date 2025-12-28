CREATE TABLE [tpm].[Partnership] (
    [PartnershipId] INT IDENTITY (1, 1) NOT NULL,
    [PartnerAId]    INT NOT NULL,
    [PartnerBId]    INT NOT NULL,
    CONSTRAINT [PK_Partnership] PRIMARY KEY CLUSTERED ([PartnershipId] ASC),
    CONSTRAINT [FK_Partnership_PartnerA] FOREIGN KEY ([PartnerAId]) REFERENCES [tpm].[Partner] ([PartnerId]),
    CONSTRAINT [FK_Partnership_PartnerB] FOREIGN KEY ([PartnerBId]) REFERENCES [tpm].[Partner] ([PartnerId])
);

