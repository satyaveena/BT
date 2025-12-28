CREATE TABLE [dbo].[bam_ResendTrackingActivity_Completed] (
    [RecordID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]       NVARCHAR (128) NOT NULL,
    [CorrelationId]    NVARCHAR (255) NULL,
    [AdapterPrefix]    NVARCHAR (255) NULL,
    [ResendCount]      INT            NULL,
    [MaxResendCount]   INT            NULL,
    [ResendInterval]   INT            NULL,
    [MaxRetryCount]    INT            NULL,
    [RetryInterval]    INT            NULL,
    [MessageContentId] NVARCHAR (255) NULL,
    [ResendTimeout]    INT            NULL,
    [RetryTimeout]     INT            NULL,
    [BTSInterchangeID] NVARCHAR (128) NULL,
    [LastModified]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_ResendTrackingActivity_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_ResendTrackingActivity_Completed]([LastModified] ASC);

