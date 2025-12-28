CREATE TABLE [dbo].[bam_ExpiredCreditCards_Active] (
    [ActivityID]      NVARCHAR (128) NOT NULL,
    [IsVisible]       BIT            DEFAULT (NULL) NULL,
    [IsComplete]      BIT            DEFAULT (NULL) NULL,
    [ID_CreditCards]  NVARCHAR (50)  NULL,
    [ID_UserObjects]  NVARCHAR (50)  NULL,
    [DateCreated]     DATETIME       NULL,
    [Last4Digits]     NVARCHAR (4)   NULL,
    [ExpirationMonth] NVARCHAR (2)   NULL,
    [ExpirationYear]  NVARCHAR (4)   NULL,
    [Alias]           NVARCHAR (50)  NULL,
    [EmailAddress]    NVARCHAR (50)  NULL,
    [LastModified]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

