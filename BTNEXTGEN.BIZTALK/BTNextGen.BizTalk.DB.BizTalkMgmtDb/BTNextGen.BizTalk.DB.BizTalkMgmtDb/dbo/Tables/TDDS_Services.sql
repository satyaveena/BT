CREATE TABLE [dbo].[TDDS_Services] (
    [ServiceID]  UNIQUEIDENTIFIER NOT NULL,
    [ServerName] NVARCHAR (32)    NOT NULL,
    PRIMARY KEY CLUSTERED ([ServiceID] ASC)
);

