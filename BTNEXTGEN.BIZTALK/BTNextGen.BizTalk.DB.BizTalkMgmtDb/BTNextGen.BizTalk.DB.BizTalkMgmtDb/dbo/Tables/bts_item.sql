CREATE TABLE [dbo].[bts_item] (
    [id]          INT              IDENTITY (1, 1) NOT NULL,
    [AssemblyId]  INT              NOT NULL,
    [Namespace]   NVARCHAR (256)   NULL,
    [Name]        NVARCHAR (256)   NOT NULL,
    [FullName]    AS               (case when [Namespace] IS NULL then [Name] else ([Namespace]+N'.')+[Name] end),
    [Type]        NVARCHAR (50)    NOT NULL,
    [IsPipeline]  TINYINT          NULL,
    [Guid]        UNIQUEIDENTIFIER NULL,
    [SchemaType]  TINYINT          NULL,
    [description] NVARCHAR (1024)  NULL,
    CONSTRAINT [PK_bts_item] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [fk_bts_item_bts_assembly] FOREIGN KEY ([AssemblyId]) REFERENCES [dbo].[bts_assembly] ([nID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_item] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_item] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_item] TO [BTS_OPERATORS]
    AS [dbo];

