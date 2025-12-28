CREATE TABLE [dbo].[bam_Metadata_OperationEvents] (
    [OperationID]  BIGINT        NULL,
    [ArtifactType] NVARCHAR (30) NULL,
    [StartTime]    DATETIME      NOT NULL,
    [EndTime]      DATETIME      NULL,
    FOREIGN KEY ([OperationID]) REFERENCES [dbo].[bam_Metadata_Operations] ([OperationID]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [CIndex_OperationIDAndArtifactType]
    ON [dbo].[bam_Metadata_OperationEvents]([OperationID] ASC, [ArtifactType] ASC);

