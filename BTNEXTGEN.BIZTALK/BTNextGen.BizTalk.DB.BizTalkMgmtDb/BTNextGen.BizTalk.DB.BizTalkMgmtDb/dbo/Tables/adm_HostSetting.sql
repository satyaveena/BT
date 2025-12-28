CREATE TABLE [dbo].[adm_HostSetting] (
    [HostId]        INT            NOT NULL,
    [PropertyName]  NVARCHAR (100) NOT NULL,
    [PropertyValue] NVARCHAR (256) NOT NULL,
    CONSTRAINT [adm_HostSetting_pk] PRIMARY KEY CLUSTERED ([HostId] ASC, [PropertyName] ASC),
    CONSTRAINT [adm_HostSetting_fk_host] FOREIGN KEY ([HostId]) REFERENCES [dbo].[adm_Host] ([Id])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_HostSetting] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_HostSetting] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_HostSetting] TO [BTS_OPERATORS]
    AS [dbo];

