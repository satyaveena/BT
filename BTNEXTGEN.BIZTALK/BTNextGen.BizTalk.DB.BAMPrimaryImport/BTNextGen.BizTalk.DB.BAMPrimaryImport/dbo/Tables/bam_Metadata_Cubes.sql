CREATE TABLE [dbo].[bam_Metadata_Cubes] (
    [CubeName]         [sysname] NOT NULL,
    [ActivityViewName] [sysname] NOT NULL,
    [ViewName]         [sysname] NOT NULL,
    [ActivityName]     [sysname] NOT NULL,
    [DefinitionXml]    NTEXT     NULL,
    [CreateOlapCube]   BIT       NULL,
    PRIMARY KEY CLUSTERED ([CubeName] ASC),
    CONSTRAINT [FK_bam_Metadata_Cubes_bam_Metadata_ActivityViews] FOREIGN KEY ([ViewName], [ActivityName]) REFERENCES [dbo].[bam_Metadata_ActivityViews] ([ViewName], [ActivityName]) ON DELETE CASCADE
);

