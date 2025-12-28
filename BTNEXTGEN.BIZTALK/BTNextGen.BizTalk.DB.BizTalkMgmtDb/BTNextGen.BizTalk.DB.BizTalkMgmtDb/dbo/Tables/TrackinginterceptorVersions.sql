CREATE TABLE [dbo].[TrackinginterceptorVersions] (
    [uidRootInterceptorID] UNIQUEIDENTIFIER NOT NULL,
    [uidInterceptorID]     UNIQUEIDENTIFIER NOT NULL,
    [AssemblyName]         NVARCHAR (1024)  NOT NULL,
    [TypeName]             NVARCHAR (256)   NOT NULL,
    [dtDeploymentTime]     DATETIME         NOT NULL,
    CONSTRAINT [adm_TrackinginterceptorVersions_idx_version] PRIMARY KEY CLUSTERED ([uidInterceptorID] ASC),
    CONSTRAINT [adm_TrackinginterceptorVersions_fk_id] FOREIGN KEY ([uidRootInterceptorID]) REFERENCES [dbo].[Trackinginterceptor] ([uidInterceptorID])
);

