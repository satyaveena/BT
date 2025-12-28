CREATE TABLE [dbo].[bam_PaymentERPTxn_Active] (
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [IsVisible]    BIT            DEFAULT (NULL) NULL,
    [IsComplete]   BIT            DEFAULT (NULL) NULL,
    [rcvERPReq]    DATETIME       NULL,
    [SndTokenReq]  DATETIME       NULL,
    [GetTokenReq]  DATETIME       NULL,
    [SendCyber]    DATETIME       NULL,
    [rcvCyber]     DATETIME       NULL,
    [ProfileReq]   DATETIME       NULL,
    [ProfileResp]  DATETIME       NULL,
    [ERPSent]      DATETIME       NULL,
    [MerchantRef]  NVARCHAR (100) NULL,
    [TargetERP]    NVARCHAR (50)  NULL,
    [email]        NVARCHAR (100) NULL,
    [cardID]       NVARCHAR (100) NULL,
    [Reason]       NVARCHAR (50)  NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

