CREATE TABLE [dbo].[bts_messagetype] (
    [nID]          INT            IDENTITY (1, 1) NOT NULL,
    [nAssemblyID]  INT            NOT NULL,
    [nvcNamespace] NVARCHAR (256) NULL,
    [nvcName]      NVARCHAR (256) NOT NULL,
    [nvcFullName]  AS             (case when [nvcNamespace] IS NULL then [nvcName] else ([nvcNamespace]+N'.')+[nvcName] end),
    CONSTRAINT [PK_bts_messagetype] PRIMARY KEY CLUSTERED ([nID] ASC)
);

