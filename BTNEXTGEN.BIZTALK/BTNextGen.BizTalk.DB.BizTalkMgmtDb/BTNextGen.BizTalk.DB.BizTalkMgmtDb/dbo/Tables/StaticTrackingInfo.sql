CREATE TABLE [dbo].[StaticTrackingInfo] (
    [GroupId]                  INT              NULL,
    [strServiceName]           NVARCHAR (256)   NOT NULL,
    [uidServiceId]             UNIQUEIDENTIFIER NOT NULL,
    [uidInterceptorId]         UNIQUEIDENTIFIER NOT NULL,
    [dtDeploymentTime]         DATETIME         NOT NULL,
    [dtUndeploymentTime]       DATETIME         NULL,
    [ismsgBodyTrackingEnabled] INT              NOT NULL,
    [imgData]                  IMAGE            NULL,
    CONSTRAINT [adm_StaticTrackingInfo_pk] PRIMARY KEY CLUSTERED ([uidServiceId] ASC, [uidInterceptorId] ASC),
    CONSTRAINT [StaticTrackingInfo_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]) ON DELETE CASCADE
);

