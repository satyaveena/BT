CREATE PROCEDURE [dbo].[sp_BlockTDDS]
AS
BEGIN

 set nocount on

 declare @ServerName sysname, @DBName sysname, @tsql nvarchar(1024)

 SELECT @ServerName = TrackingDBServerName, @DBName = TrackingDBName FROM [dbo].[adm_Group] 
 WHERE TrackingDBServerName IS NOT NULL AND TrackingDBServerName != '' 

 if ( (@ServerName IS NOT NULL) AND (@DBName IS NOT NULL) )
 BEGIN
  set @tsql = '[' + @ServerName + N'].[' + @DBName + N'].[dbo].[TDDS_BlockTDDS] '
  exec (@tsql)  
 END

 return 0

END