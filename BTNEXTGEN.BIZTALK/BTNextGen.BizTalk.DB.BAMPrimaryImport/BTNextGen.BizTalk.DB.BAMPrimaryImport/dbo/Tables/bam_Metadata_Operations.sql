CREATE TABLE [dbo].[bam_Metadata_Operations] (
    [OperationID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserLogin]             [sysname]       NOT NULL,
    [OperationType]         NVARCHAR (10)   NOT NULL,
    [OriginalDefinitionXml] NTEXT           NULL,
    [WorkingDefinitionXml]  NTEXT           NULL,
    [BamConfigurationXml]   NTEXT           NOT NULL,
    [BamDefinitionFileName] NVARCHAR (1024) NULL,
    [StartTime]             DATETIME        NOT NULL,
    [EndTime]               DATETIME        NULL,
    [BamManagerFileVersion] NVARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([OperationID] ASC)
);

