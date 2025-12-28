CREATE TABLE [dbo].[bam_Metadata_Views] (
    [ViewName]      [sysname] NOT NULL,
    [ViewID]        [sysname] NOT NULL,
    [DefinitionXml] NTEXT     NULL,
    PRIMARY KEY CLUSTERED ([ViewName] ASC)
);

