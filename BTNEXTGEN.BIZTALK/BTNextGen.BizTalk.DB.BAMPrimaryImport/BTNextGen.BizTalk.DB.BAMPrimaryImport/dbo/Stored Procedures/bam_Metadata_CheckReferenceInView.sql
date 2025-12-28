CREATE PROCEDURE [dbo].[bam_Metadata_CheckReferenceInView]
(
    @refType    NVARCHAR(128),
    @refName    NVARCHAR(128),
    @refData    NVARCHAR(1024),
    @viewName   NVARCHAR(128)
)
AS
    DECLARE @getRelationSql NVARCHAR(4000)
    DECLARE @activityName NVARCHAR(128)
    DECLARE @found INT
    SET @found = 0

    CREATE TABLE #TempArtifactsCount (RowsCount INT)

    DECLARE activities CURSOR LOCAL FOR 
        SELECT    ActivityName
        FROM    [dbo].[bam_Metadata_ActivityViews] 
        WHERE    ViewName = @viewName
    OPEN activities
    FETCH NEXT FROM activities INTO @activityName
    WHILE @found = 0 AND @@fetch_status = 0
    BEGIN
        SET @getRelationSql = N'INSERT INTO #TempArtifactsCount SELECT COUNT(ActivityID) FROM [dbo].[bam_' + @activityName +
            N'_AllRelationships] WHERE ReferenceType = @referenceType AND ReferenceName = @referenceName AND ReferenceData = @referenceData'

        EXEC sp_executesql @getRelationSql, 
            N'@referenceType NVARCHAR(128), @referenceName NVARCHAR(128), @referenceData NVARCHAR(1024)', 
            @referenceType = @refType,
            @referenceName = @refName,
            @referenceData = @refData
            
        SELECT @found = COUNT(*) FROM #TempArtifactsCount WHERE RowsCount > 0
        DELETE #TempArtifactsCount

        FETCH NEXT FROM activities INTO @activityName
    END

    CLOSE activities
    DEALLOCATE activities

    SELECT @found
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_Metadata_CheckReferenceInView] TO [BAM_ManagementWS]
    AS [dbo];

