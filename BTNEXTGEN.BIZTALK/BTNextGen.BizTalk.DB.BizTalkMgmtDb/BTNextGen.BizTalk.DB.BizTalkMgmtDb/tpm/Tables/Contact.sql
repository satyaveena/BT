CREATE TABLE [tpm].[Contact] (
    [ContactId]     INT             IDENTITY (1, 1) NOT NULL,
    [AgreementId]   INT             NOT NULL,
    [Name]          NVARCHAR (256)  NULL,
    [Company]       NVARCHAR (256)  NULL,
    [JobTitle]      NVARCHAR (256)  NULL,
    [Email]         NVARCHAR (256)  NULL,
    [WebAddress]    NVARCHAR (256)  NULL,
    [BusinessPhone] NVARCHAR (256)  NULL,
    [MobilePhone]   NVARCHAR (256)  NULL,
    [Fax]           NVARCHAR (256)  NULL,
    [Address]       NVARCHAR (512)  NULL,
    [Notes]         NVARCHAR (1024) NULL,
    [version]       ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([ContactId] ASC),
    CONSTRAINT [FK_Contact_Agreement] FOREIGN KEY ([AgreementId]) REFERENCES [tpm].[Agreement] ([Id]) ON DELETE CASCADE
);

