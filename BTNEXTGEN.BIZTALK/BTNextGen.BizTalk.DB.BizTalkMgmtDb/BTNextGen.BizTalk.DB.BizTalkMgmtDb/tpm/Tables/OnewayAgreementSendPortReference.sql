CREATE TABLE [tpm].[OnewayAgreementSendPortReference] (
    [OnewayAgreementId]   INT NOT NULL,
    [SendPortReferenceId] INT NOT NULL,
    CONSTRAINT [PK_OnewayAgreementSendPortReference] PRIMARY KEY CLUSTERED ([OnewayAgreementId] ASC, [SendPortReferenceId] ASC),
    CONSTRAINT [FK_OnewayAgreementSendPortReference_OnewayAgreement] FOREIGN KEY ([OnewayAgreementId]) REFERENCES [tpm].[OnewayAgreement] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OnewayAgreementSendPortReference_SendPortReference] FOREIGN KEY ([SendPortReferenceId]) REFERENCES [tpm].[SendPortReference] ([Id])
);

