CREATE TABLE [dbo].[adm_AdapterAlias] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [AdapterId]  INT           NOT NULL,
    [AliasValue] NVARCHAR (64) NOT NULL,
    CONSTRAINT [adm_AdapterAlias_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_AdapterAlias_unique_key] UNIQUE NONCLUSTERED ([AliasValue] ASC)
);

