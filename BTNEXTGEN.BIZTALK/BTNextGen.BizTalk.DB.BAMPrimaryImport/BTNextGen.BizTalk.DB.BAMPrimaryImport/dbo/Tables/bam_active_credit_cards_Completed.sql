CREATE TABLE [dbo].[bam_active_credit_cards_Completed] (
    [RecordID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [CSCardRcv]    DATETIME       NULL,
    [CardID]       NVARCHAR (50)  NULL,
    [ERPAccountNo] NVARCHAR (50)  NULL,
    [Destination]  NVARCHAR (50)  NULL,
    [Alias]        NVARCHAR (150) NULL,
    [ERPSent]      DATETIME       NULL,
    [CSAckSent]    DATETIME       NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_active_credit_cards_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_active_credit_cards_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_active_credit_cards_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

