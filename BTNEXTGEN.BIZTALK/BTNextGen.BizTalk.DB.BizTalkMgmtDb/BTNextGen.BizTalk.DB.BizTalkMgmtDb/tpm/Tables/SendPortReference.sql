CREATE TABLE [tpm].[SendPortReference] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [PartnerId]      INT            NOT NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [SequenceNumber] INT            NOT NULL,
    [DateModified]   DATETIME       NOT NULL,
    [version]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_SendPortReference] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SendPortReference_BTSSendPort] FOREIGN KEY ([Name]) REFERENCES [dbo].[bts_sendport] ([nvcName]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SendPortReference_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [tpm].[Partner] ([PartnerId]) ON DELETE CASCADE,
    CONSTRAINT [UK_SendPortName] UNIQUE NONCLUSTERED ([PartnerId] ASC, [Name] ASC)
);

