CREATE TABLE [dbo].[bam_FirstDataRecon_Completed] (
    [RecordID]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]        NVARCHAR (128) NOT NULL,
    [ProcessAttachment] DATETIME       NULL,
    [SendTolas]         DATETIME       NULL,
    [LastModified]      DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_FirstDataRecon_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_FirstDataRecon_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_FirstDataRecon_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

