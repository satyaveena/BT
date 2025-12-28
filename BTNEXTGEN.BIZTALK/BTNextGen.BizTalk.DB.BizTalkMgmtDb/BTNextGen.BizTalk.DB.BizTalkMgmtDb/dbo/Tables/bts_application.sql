CREATE TABLE [dbo].[bts_application] (
    [nID]            INT             IDENTITY (1, 1) NOT NULL,
    [nvcName]        NVARCHAR (256)  NOT NULL,
    [isDefault]      BIT             NOT NULL,
    [isSystem]       BIT             NOT NULL,
    [nvcDescription] NVARCHAR (1024) NULL,
    [DateModified]   DATETIME        NOT NULL,
    CONSTRAINT [bts_application_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_application_unique_name] UNIQUE NONCLUSTERED ([nvcName] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_application] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_application] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_application] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application] TO [BTS_OPERATORS]
    AS [dbo];

