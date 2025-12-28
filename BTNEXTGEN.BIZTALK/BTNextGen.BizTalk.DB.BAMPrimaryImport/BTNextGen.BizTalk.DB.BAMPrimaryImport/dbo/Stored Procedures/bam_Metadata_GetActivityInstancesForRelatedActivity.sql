CREATE PROCEDURE [dbo].[bam_Metadata_GetActivityInstancesForRelatedActivity]
(
    @viewName NVARCHAR(128),
    @relatedActivityName NVARCHAR(128),
    @relatedActivityId NVARCHAR(128)
)
AS
    DECLARE @getRelationSql NVARCHAR(4000)
    DECLARE @activityName NVARCHAR(128)

    DECLARE activities CURSOR LOCAL FOR 
        SELECT    ActivityName
        FROM    [dbo].[bam_Metadata_ActivityViews] 
        WHERE    ViewName = @viewName
    OPEN activities
    FETCH NEXT FROM activities INTO @activityName
    WHILE @@fetch_status = 0
    BEGIN
        SET @getRelationSql = N'SELECT @actName, ActivityID FROM [dbo].[bam_' + @activityName + 
                N'_AllRelationships] WHERE ReferenceName = @relActName AND ReferenceData = @relActId AND ReferenceType = N''Activity'''

        EXEC sp_executesql @getRelationSql,
                N'@actName NVARCHAR(128), @relActName NVARCHAR(128), @relActId NVARCHAR(128)',
                @actName = @activityName,
                @relActName = @relatedActivityName,
                @relActId = @relatedActivityId

        FETCH NEXT FROM activities INTO @activityName
    END

    CLOSE activities
    DEALLOCATE activities
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetActivityInstancesForRelatedActivity] TO [BAM_ManagementWS]
    AS [dbo];

