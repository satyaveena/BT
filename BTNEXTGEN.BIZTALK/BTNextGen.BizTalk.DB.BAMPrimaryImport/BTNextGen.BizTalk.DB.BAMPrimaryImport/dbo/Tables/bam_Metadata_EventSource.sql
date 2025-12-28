CREATE TABLE [dbo].[bam_Metadata_EventSource] (
    [EventSourceName] NVARCHAR (128) NOT NULL,
    [TechnologyName]  NVARCHAR (10)  COLLATE Latin1_General_BIN NOT NULL,
    [Manifest]        NVARCHAR (440) COLLATE Latin1_General_BIN NOT NULL,
    [EventSourceXml]  NTEXT          NOT NULL,
    [Version]         INT            NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [EventSourceManifestIndex]
    ON [dbo].[bam_Metadata_EventSource]([TechnologyName] ASC, [Manifest] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [EventSourceIndex]
    ON [dbo].[bam_Metadata_EventSource]([EventSourceName] ASC);

