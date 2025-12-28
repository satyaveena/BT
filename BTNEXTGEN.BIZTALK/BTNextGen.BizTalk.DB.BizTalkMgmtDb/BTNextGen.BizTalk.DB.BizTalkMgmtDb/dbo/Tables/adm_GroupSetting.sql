CREATE TABLE [dbo].[adm_GroupSetting] (
    [GroupId]       INT            NOT NULL,
    [PropertyName]  NVARCHAR (100) NOT NULL,
    [PropertyValue] NVARCHAR (256) NOT NULL,
    CONSTRAINT [adm_GroupSetting_pk] PRIMARY KEY CLUSTERED ([GroupId] ASC, [PropertyName] ASC),
    CONSTRAINT [adm_GroupSetting_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_GroupSetting] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_GroupSetting] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_GroupSetting] TO [BTS_OPERATORS]
    AS [dbo];

