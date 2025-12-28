CREATE TABLE [dbo].[Trackinginterceptor] (
    [uidInterceptorID] UNIQUEIDENTIFIER NOT NULL,
    [InterceptorType]  INT              NOT NULL,
    [AssemblyName]     NVARCHAR (256)   NOT NULL,
    [TypeName]         NVARCHAR (256)   NOT NULL,
    CONSTRAINT [adm_Trackinginterceptor_pk] PRIMARY KEY CLUSTERED ([uidInterceptorID] ASC)
);

