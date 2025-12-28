CREATE TABLE [tpm].[CustomSettings] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Blob]     VARBINARY (MAX) NULL,
    [Modified] BIT             NOT NULL,
    [version]  ROWVERSION      NOT NULL,
    CONSTRAINT [PK_CustomSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

