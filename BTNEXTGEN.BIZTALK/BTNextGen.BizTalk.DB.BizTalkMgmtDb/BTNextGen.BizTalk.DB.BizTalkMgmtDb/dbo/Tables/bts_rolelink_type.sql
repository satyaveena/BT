CREATE TABLE [dbo].[bts_rolelink_type] (
    [nID]          INT            IDENTITY (1, 1) NOT NULL,
    [nAssemblyID]  INT            NOT NULL,
    [nvcNamespace] NVARCHAR (256) NULL,
    [nvcName]      NVARCHAR (256) NOT NULL,
    [nvcFullName]  AS             (case when [nvcNamespace] IS NULL then [nvcName] else ([nvcNamespace]+N'.')+[nvcName] end),
    CONSTRAINT [PK_bts_rolelink_type] PRIMARY KEY CLUSTERED ([nID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink_type] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink_type] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink_type] TO [BTS_OPERATORS]
    AS [dbo];

