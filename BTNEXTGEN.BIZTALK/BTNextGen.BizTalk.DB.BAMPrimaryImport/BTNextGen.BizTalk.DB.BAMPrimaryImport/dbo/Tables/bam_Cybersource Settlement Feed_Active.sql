CREATE TABLE [dbo].[bam_Cybersource Settlement Feed_Active] (
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [IsVisible]    BIT            DEFAULT (NULL) NULL,
    [IsComplete]   BIT            DEFAULT (NULL) NULL,
    [TimeStarted]  DATETIME       NULL,
    [TimeEnded]    DATETIME       NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

