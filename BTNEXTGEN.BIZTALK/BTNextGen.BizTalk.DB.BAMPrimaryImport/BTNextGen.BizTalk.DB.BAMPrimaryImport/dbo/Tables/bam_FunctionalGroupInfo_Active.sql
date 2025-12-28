CREATE TABLE [dbo].[bam_FunctionalGroupInfo_Active] (
    [ActivityID]            NVARCHAR (128) NOT NULL,
    [IsVisible]             BIT            DEFAULT (NULL) NULL,
    [IsComplete]            BIT            DEFAULT (NULL) NULL,
    [InterchangeActivityID] NVARCHAR (128) NULL,
    [GroupControlNo]        NVARCHAR (14)  NULL,
    [FunctionalIDCode]      NVARCHAR (6)   NULL,
    [TSCount]               INT            NULL,
    [LastModified]          DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

