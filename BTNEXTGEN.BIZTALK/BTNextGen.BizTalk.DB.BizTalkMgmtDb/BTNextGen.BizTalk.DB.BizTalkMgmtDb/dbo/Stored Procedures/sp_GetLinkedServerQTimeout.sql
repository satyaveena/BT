CREATE PROCEDURE [dbo].[sp_GetLinkedServerQTimeout] @server sysname
AS
BEGIN
 set nocount on

 
 DECLARE @timeout int
 
 SELECT @timeout = querytimeout
 FROM master.dbo.sysservers
 WHERE srvname=@server
 
 IF @timeout > 0
  GOTO DONE
 
 IF OBJECT_ID( 'tempdb..#temp' ) IS NOT NULL
  DROP TABLE #temp
 
 CREATE TABLE #temp
 (
  [name]   nvarchar(70)
  ,minimum  int
  ,maximum  int
  ,config_value int -- what has been set
  ,run_value  int -- what is actually currently being used by the system
 )
 
 INSERT #temp EXEC sp_configure 'query timeout' 
 
 SELECT @timeout = run_value
 FROM #temp
 
 IF @timeout IS NULL OR @timeout < 0
  GOTO ERROR
 ELSE
  GOTO DONE
 
 ERROR:
  SET @timeout = -1
 
 DONE:
  IF OBJECT_ID( 'tempdb..#temp' ) IS NOT NULL
   DROP TABLE #temp
 
  RETURN @timeout
END

GRANT EXECUTE ON [dbo].[sp_GetLinkedServerQTimeout] TO BTS_BACKUP_USERS