CREATE PROCEDURE [dbo].[bam_Metadata_GetRelatedActivityInstances]
(
    @viewName NVARCHAR(128),
    @activityName NVARCHAR(128),
    @activityId NVARCHAR(128)
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    -- Instances which don't belong to the view are filtered out in the code
    DECLARE @getRelationSql NVARCHAR(4000)
    SET @getRelationSql = N'SELECT ReferenceName, ReferenceData FROM [dbo].[bam_' + @activityName + 
        N'_AllRelationships] WHERE ActivityID = @actIdParam AND ReferenceType = N''Activity'''

    EXEC sp_executesql @getRelationSql,
        N'@actIdParam NVARCHAR(128)',
        @actIdParam = @activityId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetRelatedActivityInstances] TO [BAM_ManagementWS]
    AS [dbo];

