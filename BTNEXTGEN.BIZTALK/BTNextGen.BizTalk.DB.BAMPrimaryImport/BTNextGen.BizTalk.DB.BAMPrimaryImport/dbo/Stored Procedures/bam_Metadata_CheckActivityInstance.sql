CREATE PROCEDURE [dbo].[bam_Metadata_CheckActivityInstance]
(
    @viewName sysname,
    @activityName sysname,
    @activityInstanceID NVARCHAR(128)
)
AS
    DECLARE @activityViewName sysname
    
    SELECT @activityViewName = ActivityViewName
    FROM bam_Metadata_ActivityViews
    WHERE ActivityName = @activityName AND ViewName = @viewName

    IF (@activityViewName IS NOT NULL)
    BEGIN
        DECLARE @sqlViewName NVARCHAR(500)
        SELECT @sqlViewName = N'bam_' + @viewName + N'_' + @activityViewName + N'_View'

        DECLARE @sqlQuery NVARCHAR(600)
        SELECT @sqlQuery = N'SELECT COUNT(*) FROM [' + @sqlViewName + N'] WHERE ActivityID = @activityID'

        EXEC sp_executesql @sqlQuery,
            N'@activityID NVARCHAR(128)',
            @activityID = @activityInstanceID
    END
    ELSE
        SELECT (-1)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_CheckActivityInstance] TO [BAM_ManagementWS]
    AS [dbo];

