CREATE TABLE [dbo].[adm_Adapter] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (256)   NOT NULL,
    [Capabilities]         INT              DEFAULT ((0)) NOT NULL,
    [Comment]              NVARCHAR (256)   NOT NULL,
    [DateModified]         DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [MgmtCLSID]            UNIQUEIDENTIFIER NOT NULL,
    [InboundEngineCLSID]   UNIQUEIDENTIFIER NULL,
    [InboundAssemblyPath]  NVARCHAR (256)   NULL,
    [InboundTypeName]      NVARCHAR (256)   NULL,
    [OutboundEngineCLSID]  UNIQUEIDENTIFIER NULL,
    [OutboundAssemblyPath] NVARCHAR (256)   NULL,
    [OutboundTypeName]     NVARCHAR (256)   NULL,
    [PropertyNameSpace]    NVARCHAR (256)   NOT NULL,
    [DefaultRHCfg]         NTEXT            NULL,
    [DefaultTHCfg]         NTEXT            NULL,
    CONSTRAINT [adm_Adapter_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_Adapter_unique_key1] UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [adm_Adapter_unique_key2] UNIQUE NONCLUSTERED ([MgmtCLSID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Adapter] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Adapter] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Adapter] TO [BTS_OPERATORS]
    AS [dbo];

