CREATE TABLE [dbo].[bam_BusinessMessageJournal_Active] (
    [ActivityID]              NVARCHAR (128) NOT NULL,
    [IsVisible]               BIT            DEFAULT (NULL) NULL,
    [IsComplete]              BIT            DEFAULT (NULL) NULL,
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
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

