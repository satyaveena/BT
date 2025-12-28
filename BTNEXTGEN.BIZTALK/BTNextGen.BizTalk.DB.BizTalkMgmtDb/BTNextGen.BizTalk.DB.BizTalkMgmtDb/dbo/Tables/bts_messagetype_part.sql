CREATE TABLE [dbo].[bts_messagetype_part] (
    [nID]                   INT            IDENTITY (1, 1) NOT NULL,
    [nMessageTypeID]        INT            NOT NULL,
    [nvcNamespace]          NVARCHAR (256) NULL,
    [nvcName]               NVARCHAR (256) NOT NULL,
    [nvcFullName]           AS             (case when [nvcNamespace] IS NULL then [nvcName] else ([nvcNamespace]+N'.')+[nvcName] end),
    [nvcSchemaURTNameSpace] NVARCHAR (256) NULL,
    [nvcSchemaURTTypeName]  NVARCHAR (256) NULL,
    CONSTRAINT [PK_bts_messagetype_part] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_msgtype_part_bts_messagetype] FOREIGN KEY ([nMessageTypeID]) REFERENCES [dbo].[bts_messagetype] ([nID]) ON DELETE CASCADE
);

