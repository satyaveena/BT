CREATE TABLE [dbo].[bts_assembly] (
    [nID]                INT            IDENTITY (1, 1) NOT NULL,
    [nvcName]            NVARCHAR (256) NOT NULL,
    [nvcVersion]         NVARCHAR (256) NOT NULL,
    [nvcCulture]         NVARCHAR (256) CONSTRAINT [DF_bts_assembly_nvcCulture] DEFAULT (N'neutral') NULL,
    [nvcPublicKeyToken]  NVARCHAR (256) CONSTRAINT [DF_bts_assembly_nvcPublicKeyToken] DEFAULT (N'') NULL,
    [nvcFullName]        NVARCHAR (256) NOT NULL,
    [nVersionMajor]      INT            NOT NULL,
    [nVersionMinor]      INT            NOT NULL,
    [nVersionBuild]      INT            NOT NULL,
    [nVersionRevision]   INT            NOT NULL,
    [dtDateModified]     DATETIME       NOT NULL,
    [nvcModifiedBy]      NVARCHAR (64)  NOT NULL,
    [nType]              INT            NOT NULL,
    [nGroupId]           INT            NULL,
    [nvcDescription]     NVARCHAR (256) CONSTRAINT [DF_bts_assembly_nvcDescription] DEFAULT (N'') NULL,
    [nvcIdentity]        NVARCHAR (256) CONSTRAINT [DF_bts_assembly_nvcIdentity] DEFAULT (N'') NULL,
    [nvcType]            NVARCHAR (256) CONSTRAINT [DF_bts_assembly_nvcType] DEFAULT (N'Assembly') NULL,
    [nStrongName]        TINYINT        CONSTRAINT [DF_bts_assembly_nStrongName] DEFAULT ((1)) NULL,
    [ntxtModuleXML]      NTEXT          NULL,
    [imgTrackingProfile] IMAGE          NULL,
    [nSystemAssembly]    INT            CONSTRAINT [DF_bts_assembly_nSystemAssembly] DEFAULT ((0)) NOT NULL,
    [nApplicationID]     INT            NOT NULL,
    CONSTRAINT [PK_bts_assembly] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_assembly_foreign_applicationid] FOREIGN KEY ([nApplicationID]) REFERENCES [dbo].[bts_application] ([nID])
);


GO
CREATE NONCLUSTERED INDEX [IX_bts_assembly]
    ON [dbo].[bts_assembly]([nvcName] ASC, [nVersionMajor] ASC, [nVersionMinor] ASC, [nVersionBuild] ASC, [nVersionRevision] ASC, [nID] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_assembly] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_assembly] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_assembly] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_assembly] TO [BTS_OPERATORS]
    AS [dbo];

