CREATE TABLE [dbo].[btsmon_Inconsistancies] (
    [DBServer]     [sysname] NOT NULL,
    [DBName]       [sysname] NOT NULL,
    [nProblemCode] SMALLINT  NOT NULL,
    [nCount]       BIGINT    NOT NULL
);

