CREATE TABLE [dbo].[bam_EmailProcess_EmailProcess_RTATable] (
    [Partition]        INT            NULL,
    [Timeslice]        DATETIME       NULL,
    [EmailConstructed] FLOAT (53)     NULL,
    [ProcessTime]      FLOAT (53)     NULL,
    [_Count]           BIGINT         NULL,
    [EmailProgress]    NVARCHAR (100) NULL
);


GO
CREATE CLUSTERED INDEX [CI]
    ON [dbo].[bam_EmailProcess_EmailProcess_RTATable]([Partition] ASC, [Timeslice] ASC, [EmailProgress] ASC);

