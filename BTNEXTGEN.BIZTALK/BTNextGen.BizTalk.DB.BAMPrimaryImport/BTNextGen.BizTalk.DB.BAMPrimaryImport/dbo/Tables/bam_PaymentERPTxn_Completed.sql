CREATE TABLE [dbo].[bam_PaymentERPTxn_Completed] (
    [RecordID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]   NVARCHAR (128) NOT NULL,
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
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_PaymentERPTxn_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_PaymentERPTxn_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_PaymentERPTxn_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

