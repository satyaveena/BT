/*****************************************************************************************************
		[dbo].[admv_BackupDatabases]
	View of all databases which we need to backup
***************************************************************************************************/
CREATE VIEW [dbo].[admv_BackupDatabases]
AS
	SELECT CAST(SERVERPROPERTY('servername') as sysname) as ServerName, DB_NAME() as DatabaseName
	
	UNION
	
	SELECT TrackingDBServerName as ServerName, TrackingDBName as DatabaseName FROM [dbo].[adm_Group] 
	WHERE TrackingDBServerName IS NOT NULL AND TrackingDBServerName != '' 
	UNION
	
	SELECT BamDBServerName as ServerName, BamDBName as DatabaseName FROM [dbo].[adm_Group] 
	WHERE BamDBServerName IS NOT NULL AND BamDBServerName != ''
	
	UNION
	
	SELECT RuleEngineDBServerName as ServerName, RuleEngineDBName as DatabaseName FROM [dbo].[adm_Group] 
	WHERE RuleEngineDBServerName IS NOT NULL AND RuleEngineDBServerName != ''
	
	UNION
	SELECT 	DBServerName as ServerName, DBName as DatabaseName FROM [dbo].[adm_MessageBox]
	WHERE DBServerName IS NOT NULL AND DBServerName != ''
	
	UNION 
	
	SELECT ServerName, DatabaseName	FROM [dbo].[adm_OtherBackupDatabases]
	WHERE ServerName IS NOT NULL AND ServerName != ''
	GROUP BY ServerName, DatabaseName --group by here so we can handle dupe rows
