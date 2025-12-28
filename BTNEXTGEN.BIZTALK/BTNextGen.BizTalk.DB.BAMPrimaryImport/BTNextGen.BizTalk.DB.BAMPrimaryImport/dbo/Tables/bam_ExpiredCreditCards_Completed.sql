CREATE TABLE [dbo].[bam_ExpiredCreditCards_Completed] (
    [RecordID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]      NVARCHAR (128) NOT NULL,
    [ID_CreditCards]  NVARCHAR (50)  NULL,
    [ID_UserObjects]  NVARCHAR (50)  NULL,
    [DateCreated]     DATETIME       NULL,
    [Last4Digits]     NVARCHAR (4)   NULL,
    [ExpirationMonth] NVARCHAR (2)   NULL,
    [ExpirationYear]  NVARCHAR (4)   NULL,
    [Alias]           NVARCHAR (50)  NULL,
    [EmailAddress]    NVARCHAR (50)  NULL,
    [LastModified]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_ExpiredCreditCards_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_ExpiredCreditCards_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ExpiredCreditCards_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

