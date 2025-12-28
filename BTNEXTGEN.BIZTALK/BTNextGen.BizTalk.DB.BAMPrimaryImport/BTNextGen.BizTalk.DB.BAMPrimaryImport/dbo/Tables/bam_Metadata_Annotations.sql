CREATE TABLE [dbo].[bam_Metadata_Annotations] (
    [ActivityName]  [sysname]        NOT NULL,
    [Version]       UNIQUEIDENTIFIER NOT NULL,
    [MinorVersion]  INT              NOT NULL,
    [Subject]       NVARCHAR (256)   NOT NULL,
    [Component]     NVARCHAR (256)   NOT NULL,
    [TrackPointId]  INT              NOT NULL,
    [AnnotationXml] NTEXT            NULL
);


GO
CREATE CLUSTERED INDEX [NCIndex_VersionAndMinorVersionAndTrackPointId]
    ON [dbo].[bam_Metadata_Annotations]([Version] ASC, [MinorVersion] ASC, [TrackPointId] ASC);

