CREATE TABLE [dbo].[bts_porttype] (
    [nID]          INT            IDENTITY (1, 1) NOT NULL,
    [nAssemblyID]  INT            NOT NULL,
    [nvcNamespace] NVARCHAR (256) NULL,
    [nvcName]      NVARCHAR (256) NOT NULL,
    [nvcFullName]  AS             (case when [nvcNamespace] IS NULL then [nvcName] else ([nvcNamespace]+N'.')+[nvcName] end),
    CONSTRAINT [PK_bts_porttype] PRIMARY KEY CLUSTERED ([nID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_porttype] TO [BTS_OPERATORS]
    AS [dbo];

