CREATE TABLE [dbo].[bam_FirstDataRecon_Active] (
    [ActivityID]        NVARCHAR (128) NOT NULL,
    [IsVisible]         BIT            DEFAULT (NULL) NULL,
    [IsComplete]        BIT            DEFAULT (NULL) NULL,
    [ProcessAttachment] DATETIME       NULL,
    [SendTolas]         DATETIME       NULL,
    [LastModified]      DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

