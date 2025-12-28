CREATE TABLE [dbo].[bam_Metadata_ActivityViews] (
    [ActivityViewName] [sysname] NOT NULL,
    [ViewName]         [sysname] NOT NULL,
    [ActivityName]     [sysname] NOT NULL,
    CONSTRAINT [PK_bam_Metadata_ActivityViews] PRIMARY KEY CLUSTERED ([ViewName] ASC, [ActivityName] ASC),
    FOREIGN KEY ([ActivityName]) REFERENCES [dbo].[bam_Metadata_Activities] ([ActivityName]) ON DELETE CASCADE,
    FOREIGN KEY ([ViewName]) REFERENCES [dbo].[bam_Metadata_Views] ([ViewName]) ON DELETE CASCADE
);

