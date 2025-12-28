CREATE PROCEDURE [dbo].[btsmon_LoadIssues]
AS
 SELECT inc.DBServer AS N'Server', inc.DBName AS N'Database', iss.nvcProblemDescription AS N'Problem Description', inc.nCount AS N'Count'
 FROM [dbo].[btsmon_Inconsistancies] inc JOIN [dbo].[btsmon_Issues] iss ON iss.[nProblemCode] = inc.[nProblemCode]
 ORDER BY inc.nCount DESC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[btsmon_LoadIssues] TO [BTS_ADMIN_USERS]
    AS [dbo];

