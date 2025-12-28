CREATE TABLE [dbo].[bam_APIDemo_Completed] (
    [RecordID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [Key]          NVARCHAR (50)  NULL,
    [Data1]        INT            NULL,
    [Data2]        INT            NULL,
    [Data3]        INT            NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_APIDemo_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_APIDemo_Completed]([LastModified] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_APIDemo_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

