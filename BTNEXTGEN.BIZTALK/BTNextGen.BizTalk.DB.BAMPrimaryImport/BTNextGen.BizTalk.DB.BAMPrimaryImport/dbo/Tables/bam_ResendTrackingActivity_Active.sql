CREATE TABLE [dbo].[bam_ResendTrackingActivity_Active] (
    [ActivityID]       NVARCHAR (128) NOT NULL,
    [IsVisible]        BIT            DEFAULT (NULL) NULL,
    [IsComplete]       BIT            DEFAULT (NULL) NULL,
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
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

