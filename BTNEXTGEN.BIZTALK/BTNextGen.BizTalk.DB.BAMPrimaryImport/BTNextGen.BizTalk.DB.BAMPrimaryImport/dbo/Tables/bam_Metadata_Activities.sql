CREATE TABLE [dbo].[bam_Metadata_Activities] (
    [ActivityName]           [sysname] NOT NULL,
    [DefinitionXml]          NTEXT     NULL,
    [OnlineWindowTimeUnit]   CHAR (10) NULL,
    [OnlineWindowTimeLength] INT       NULL,
    [Archive]                TINYINT   DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([ActivityName] ASC),
    CHECK ([OnlineWindowTimeUnit]='MINUTE' OR [OnlineWindowTimeUnit]='HOUR' OR [OnlineWindowTimeUnit]='DAY' OR [OnlineWindowTimeUnit]='MONTH')
);

