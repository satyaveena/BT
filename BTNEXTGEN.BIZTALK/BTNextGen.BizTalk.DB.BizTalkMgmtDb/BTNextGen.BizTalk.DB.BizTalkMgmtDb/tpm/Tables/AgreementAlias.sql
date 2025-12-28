CREATE TABLE [tpm].[AgreementAlias] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [OnewayAgreementId] INT            NOT NULL,
    [Protocol]          NVARCHAR (64)  NOT NULL,
    [Key]               NVARCHAR (64)  NOT NULL,
    [Value]             NVARCHAR (256) NOT NULL,
    [version]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_OnewayAgreementAlias] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AgreementAlias_OnewayAgreement] FOREIGN KEY ([OnewayAgreementId]) REFERENCES [tpm].[OnewayAgreement] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [UK_AgreementAlias] UNIQUE NONCLUSTERED ([Key] ASC, [Value] ASC, [Protocol] ASC)
);


GO
CREATE CLUSTERED INDEX [IX_AgreementAlias_OnewayAgreementId]
    ON [tpm].[AgreementAlias]([OnewayAgreementId] ASC);

