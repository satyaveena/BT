CREATE PROCEDURE [dbo].[bam_Metadata_GetReferences]
(
    @activityName NVARCHAR(128),
    @activityId NVARCHAR(128),
    @referenceType NVARCHAR(128) = NULL
)
AS
    IF NOT EXISTS(SELECT ActivityName FROM bam_Metadata_Activities WHERE ActivityName = @activityName)
    BEGIN
        RAISERROR (N'ActivityDoesNotExist', 16, 1)
        RETURN
    END

    DECLARE @getReferenceSql NVARCHAR(4000)
    IF (@referenceType is NULL)
    BEGIN
        SET @getReferenceSql = N'SELECT ReferenceType, ReferenceName, ReferenceData, LongReferenceData FROM [dbo].[bam_' 
            + @activityName + N'_AllRelationships] WHERE ActivityID = @actInstanceId'

        EXEC sp_executesql @getReferenceSql,
            N'@actInstanceId NVARCHAR(128)',
            @activityId
    END
    ELSE
    BEGIN
        SET @getReferenceSql = N'SELECT ReferenceType, ReferenceName, ReferenceData, LongReferenceData FROM [dbo].[bam_' 
            + @activityName + N'_AllRelationships] WHERE ActivityID = @actInstanceId AND ReferenceType = @refType'

        EXEC sp_executesql @getReferenceSql,
            N'@actInstanceId NVARCHAR(128), @refType NVARCHAR(128)',
            @activityId,
            @referenceType
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_GetReferences] TO [BAM_ManagementWS]
    AS [dbo];

