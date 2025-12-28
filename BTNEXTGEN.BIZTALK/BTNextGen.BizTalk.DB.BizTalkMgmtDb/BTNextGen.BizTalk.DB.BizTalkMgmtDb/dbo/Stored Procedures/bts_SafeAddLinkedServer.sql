CREATE PROCEDURE [dbo].[bts_SafeAddLinkedServer]
@linkedServer sysname
AS

declare @localServer sysname, @svr sysname


declare @localized_string_Unconfigured_Linked_Server nvarchar(128)
set @localized_string_Unconfigured_Linked_Server = N'%s was configured as a linked server as required by BizTalk Server.'

set @localServer = CAST(SERVERPROPERTY('servername') as sysname)

if ( (@linkedServer IS NULL) OR (@linkedServer = @localServer) )
BEGIN
 --this is just a noop
 return
END

CREATE TABLE #Servers (SRV_NAME sysname, SRV_PROVIDERNAME nvarchar(128) NULL, SRV_PRODUCT nvarchar(128) NULL, SRV_DATASOURCE nvarchar(4000) NULL, SRV_PROVIDERSTRING nvarchar(4000) NULL, SRV_LOCATION nvarchar(4000) NULL, SRV_CAT sysname NULL )
INSERT INTO #Servers exec sp_linkedservers

IF NOT EXISTS (SELECT TOP 1 SRV_NAME FROM #Servers WHERE SRV_NAME = @linkedServer)
BEGIN
 --they are not yet linked so we will link them
 exec sp_addlinkedserver @srvproduct = 'SQL Server', @server = @linkedServer
 RAISERROR(@localized_string_Unconfigured_Linked_Server, 10, 1)
END

DROP TABLE #Servers