CREATE TABLE [dbo].[TDDS_Settings] (
    [RefreshInterval]      INT           NULL,
    [SqlCommandTimeout]    INT           NULL,
    [SessionTimeout]       INT           NULL,
    [EventLoggingInterval] NVARCHAR (16) NULL,
    [RetryCount]           INT           NULL,
    [ThreadPerSession]     INT           NULL,
    CONSTRAINT [chk_RefreshInterval] CHECK ([RefreshInterval]>=(60)),
    CONSTRAINT [chk_Sessiontimeout] CHECK ([SessionTimeout]>(2)*[RefreshInterval]),
    CONSTRAINT [chk_Sqltimeout] CHECK ([SqlCommandTimeout]<[RefreshInterval])
);

