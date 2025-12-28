CREATE TABLE [dbo].[bam_Metadata_Properties] (
    [Scope]         [sysname] NOT NULL,
    [PropertyName]  [sysname] NOT NULL,
    [PropertyValue] NTEXT     NULL
);


GO
CREATE CLUSTERED INDEX [CI_ScopeAndPropName]
    ON [dbo].[bam_Metadata_Properties]([Scope] ASC, [PropertyName] ASC);

