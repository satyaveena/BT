CREATE TABLE [dbo].[bam_Metadata_TrackingProfiles] (
    [nId]            INT              IDENTITY (1, 1) NOT NULL,
    [ActivityName]   [sysname]        NOT NULL,
    [ReferencedBy]   NVARCHAR (300)   NULL,
    [VersionId]      UNIQUEIDENTIFIER NOT NULL,
    [MinorVersionId] INT              NOT NULL,
    [ProfileXml]     NTEXT            NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [CIndex_ActivityNameAndReferencedBy]
    ON [dbo].[bam_Metadata_TrackingProfiles]([ActivityName] ASC, [ReferencedBy] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [CIndex_VersionId]
    ON [dbo].[bam_Metadata_TrackingProfiles]([VersionId] ASC);

