CREATE TABLE [dbo].[adm_HostInstanceSetting] (
    [HostInstanceId] INT            NOT NULL,
    [PropertyName]   NVARCHAR (100) NOT NULL,
    [PropertyValue]  NVARCHAR (256) NOT NULL,
    CONSTRAINT [adm_HostInstanceSetting_pk] PRIMARY KEY CLUSTERED ([HostInstanceId] ASC, [PropertyName] ASC),
    CONSTRAINT [adm_HostInstanceSetting_fk_hostInstance] FOREIGN KEY ([HostInstanceId]) REFERENCES [dbo].[adm_HostInstance] ([Id])
);

