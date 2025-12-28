CREATE TABLE [dbo].[BT_BAMAlertHistory] (
    [Interface Name] NVARCHAR (100) NOT NULL,
    [ActivityID]     NVARCHAR (100) NOT NULL,
    [RetryCount]     INT            NULL,
    [AlertExpired]   BIT            NULL,
    [LastModified]   DATETIME       NULL,
    [KeyIdentifier]  NVARCHAR (100) NULL
);

