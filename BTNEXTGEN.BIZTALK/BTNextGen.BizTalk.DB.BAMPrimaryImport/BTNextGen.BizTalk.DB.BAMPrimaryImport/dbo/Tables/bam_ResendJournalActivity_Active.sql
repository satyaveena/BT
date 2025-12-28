CREATE TABLE [dbo].[bam_ResendJournalActivity_Active] (
    [ActivityID]         NVARCHAR (128) NOT NULL,
    [IsVisible]          BIT            DEFAULT (NULL) NULL,
    [IsComplete]         BIT            DEFAULT (NULL) NULL,
    [TrackingActivityID] NVARCHAR (255) NULL,
    [ResendIndex]        INT            NULL,
    [ResendStatus]       NVARCHAR (50)  NULL,
    [BTSInterchangeID]   NVARCHAR (128) NULL,
    [LastModified]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

