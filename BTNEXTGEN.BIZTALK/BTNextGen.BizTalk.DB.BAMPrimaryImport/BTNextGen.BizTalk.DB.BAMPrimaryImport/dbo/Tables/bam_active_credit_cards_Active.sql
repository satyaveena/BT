CREATE TABLE [dbo].[bam_active_credit_cards_Active] (
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [IsVisible]    BIT            DEFAULT (NULL) NULL,
    [IsComplete]   BIT            DEFAULT (NULL) NULL,
    [CSCardRcv]    DATETIME       NULL,
    [CardID]       NVARCHAR (50)  NULL,
    [ERPAccountNo] NVARCHAR (50)  NULL,
    [Destination]  NVARCHAR (50)  NULL,
    [Alias]        NVARCHAR (150) NULL,
    [ERPSent]      DATETIME       NULL,
    [CSAckSent]    DATETIME       NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

