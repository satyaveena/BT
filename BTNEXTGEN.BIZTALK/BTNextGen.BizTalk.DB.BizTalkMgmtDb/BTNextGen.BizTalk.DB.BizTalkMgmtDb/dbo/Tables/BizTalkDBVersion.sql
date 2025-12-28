CREATE TABLE [dbo].[BizTalkDBVersion] (
    [BizTalkDBName]       NVARCHAR (64)  NOT NULL,
    [DatabaseMajor]       INT            NOT NULL,
    [DatabaseMinor]       INT            NOT NULL,
    [DatabaseBuildNumber] INT            NOT NULL,
    [DatabaseRevision]    INT            NOT NULL,
    [ProductMajor]        INT            NOT NULL,
    [ProductMinor]        INT            NOT NULL,
    [ProductBuildNumber]  INT            NOT NULL,
    [ProductRevision]     INT            NOT NULL,
    [ProductLanguage]     NVARCHAR (256) NOT NULL,
    [Description]         NVARCHAR (256) NOT NULL,
    [Modified]            DATETIME       DEFAULT (getutcdate()) NULL,
    CONSTRAINT [BizTalkDBVersion_unique_key] UNIQUE NONCLUSTERED ([BizTalkDBName] ASC, [ProductMajor] ASC, [ProductMinor] ASC, [ProductBuildNumber] ASC, [ProductRevision] ASC)
);

