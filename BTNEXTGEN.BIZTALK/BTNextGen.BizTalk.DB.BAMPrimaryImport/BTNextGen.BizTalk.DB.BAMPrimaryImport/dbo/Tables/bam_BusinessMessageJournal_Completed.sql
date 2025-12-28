CREATE TABLE [dbo].[bam_BusinessMessageJournal_Completed] (
    [RecordID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityID]              NVARCHAR (128) NOT NULL,
    [MessageTrackingID]       NVARCHAR (128) NULL,
    [ActionType]              INT            NULL,
    [ContainerActivityID]     NVARCHAR (128) NULL,
    [ContainerType]           INT            NULL,
    [BTSInterchangeID]        NVARCHAR (128) NULL,
    [BTSMessageID]            NVARCHAR (128) NULL,
    [BTSServiceInstanceID]    NVARCHAR (128) NULL,
    [BTSHostName]             NVARCHAR (128) NULL,
    [RoutedToPartyName]       NVARCHAR (256) NULL,
    [LinkedMessageTrackingID] NVARCHAR (128) NULL,
    [TimeCreated]             DATETIME       NULL,
    [LastModified]            DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_BusinessMessageJournal_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_BusinessMessageJournal_Completed]([LastModified] ASC);

