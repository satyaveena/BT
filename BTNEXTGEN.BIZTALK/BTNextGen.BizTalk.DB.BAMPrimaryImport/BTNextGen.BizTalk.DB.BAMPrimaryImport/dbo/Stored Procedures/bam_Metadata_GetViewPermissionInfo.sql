CREATE PROCEDURE [dbo].[bam_Metadata_GetViewPermissionInfo]
AS
    SELECT SUSER_SNAME(sid) FROM dbo.sysusers WHERE name = N'dbo'
    
    SELECT ViewName = bamViews.ViewName, NTAccount = SUSER_SNAME(bamUsers.sid), IsNTGroup = bamUsers.isntgroup, 
           Activity = bamActivityViews.ActivityName
    FROM [dbo].[bam_Metadata_Views] bamViews
        JOIN dbo.sysusers viewRole ON viewRole.name = 'bam_' + bamViews.ViewName
        LEFT JOIN dbo.sysmembers bamMembers ON viewRole.uid = bamMembers.groupuid
        LEFT JOIN dbo.sysusers bamUsers ON bamMembers.memberuid = bamUsers.uid
        JOIN [dbo].[bam_Metadata_ActivityViews] bamActivityViews ON bamActivityViews.ViewName = bamViews.ViewName
    WHERE (bamUsers.issqlrole = 1 OR (bamUsers.islogin = 1 AND bamUsers.issqluser = 0 AND bamUsers.hasdbaccess = 1)) 
        AND bamViews.ViewName IS NOT NULL
    ORDER BY bamViews.ViewName

    SELECT ViewName = bamViews.ViewName, 
           Activity = bamActivityViews.ActivityName, CubeName = bamCubes.CubeName, CreateOlapCube = bamCubes.CreateOlapCube, RtaName = bamRTA.RtaName, RtaWindow = bamRTA.RTAWindow
    FROM [dbo].[bam_Metadata_Views] bamViews
        JOIN [dbo].[bam_Metadata_ActivityViews] bamActivityViews ON bamActivityViews.ViewName = bamViews.ViewName
        LEFT JOIN [dbo].[bam_Metadata_Cubes] bamCubes ON (bamActivityViews.ViewName = bamCubes.ViewName AND bamActivityViews.ActivityName = bamCubes.ActivityName)
        LEFT JOIN [dbo].[bam_Metadata_RealTimeAggregations] bamRTA ON bamCubes.CubeName = bamRTA.CubeName
    WHERE bamViews.ViewName IS NOT NULL
    ORDER BY bamViews.ViewName, bamActivityViews.ActivityName
    
    SELECT DISTINCT CubeName, PivotTableName, RtaName
    FROM [dbo].[bam_Metadata_PivotTables]
    ORDER BY CubeName, RtaName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetViewPermissionInfo] TO [BAM_ManagementWS]
    AS [dbo];

