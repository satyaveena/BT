CREATE TABLE [dbo].[bam_ERPOrders_Completed] (
    [RecordID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]      NVARCHAR (128) NOT NULL,
    [PONum]           NVARCHAR (50)  NULL,
    [TransNum]        NVARCHAR (50)  NULL,
    [AccountNum]      NVARCHAR (50)  NULL,
    [TargetERP]       NVARCHAR (20)  NULL,
    [PORcvd]          DATETIME       NULL,
    [POSentERP]       DATETIME       NULL,
    [ERPAckRcv]       DATETIME       NULL,
    [NGRHeaderUpdate] DATETIME       NULL,
    [NGRLineUpdate]   DATETIME       NULL,
    [CSAckUpdate]     DATETIME       NULL,
    [LastModified]    DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_ERPOrders_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_ERPOrders_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_ERPOrders_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

