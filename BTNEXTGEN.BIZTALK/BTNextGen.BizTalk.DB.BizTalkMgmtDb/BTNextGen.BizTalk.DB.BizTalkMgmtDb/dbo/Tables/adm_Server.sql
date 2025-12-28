CREATE TABLE [dbo].[adm_Server] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (63) NOT NULL,
    [DateModified] DATETIME      DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [adm_Server_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_Server_unique_key] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Server] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Server] TO [BTS_OPERATORS]
    AS [dbo];

