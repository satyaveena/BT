CREATE TABLE [dbo].[bam_APIDemo_Active] (
    [ActivityID]   NVARCHAR (128) NOT NULL,
    [IsVisible]    BIT            DEFAULT (NULL) NULL,
    [IsComplete]   BIT            DEFAULT (NULL) NULL,
    [Key]          NVARCHAR (50)  NULL,
    [Data1]        INT            NULL,
    [Data2]        INT            NULL,
    [Data3]        INT            NULL,
    [LastModified] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);

