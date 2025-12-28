CREATE TABLE [dbo].[edi_DbConfig] (
    [EdiRuntimeDbServerName] NVARCHAR (80)  NULL,
    [EdiRuntimeDbName]       NVARCHAR (128) NULL,
    [EdiEnabled]             BIT            DEFAULT ((0)) NOT NULL,
    [AS2Enabled]             BIT            DEFAULT ((0)) NOT NULL,
    [ReportingEnabled]       BIT            DEFAULT ((0)) NOT NULL,
    [EdiConfigured]          BIT            DEFAULT ((0)) NOT NULL,
    [AS2Configured]          BIT            DEFAULT ((0)) NOT NULL,
    [ReportingConfigured]    BIT            DEFAULT ((0)) NOT NULL,
    [SsoApplicationName]     NVARCHAR (256) NULL
);

