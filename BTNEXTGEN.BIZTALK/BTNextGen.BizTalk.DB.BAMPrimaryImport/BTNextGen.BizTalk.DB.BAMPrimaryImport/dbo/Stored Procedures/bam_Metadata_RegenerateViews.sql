-- Utility procedure called by bam_Metadata_EndArchiving and bam_Metadata_SpawnPartition
-- to regenerates completed instances and all relationship views
CREATE PROCEDURE [dbo].[bam_Metadata_RegenerateViews]
(
    @activityName NVARCHAR(256),
    @isArchivingView BIT = 0,
    -- following two arguements are only used when isArchivingView is 1
    @partitionOnlineThreshold DATETIME = NULL,
    @analysisRecordWatermark BIGINT = NULL
)
AS
    -- View definition buffers
    DECLARE @@instanceViewSql NVARCHAR(4000)
    DECLARE @@instanceViewSql1 NVARCHAR(4000)
    DECLARE @@instanceViewSql2 NVARCHAR(4000)
    DECLARE @@instanceViewSql3 NVARCHAR(4000)
    DECLARE @@instanceViewSql4 NVARCHAR(4000)
    DECLARE @@instanceViewSql5 NVARCHAR(4000)
    DECLARE @@instanceViewSql6 NVARCHAR(4000)
    DECLARE @@instanceViewSql7 NVARCHAR(4000)
    DECLARE @@instanceViewSql8 NVARCHAR(4000)
    DECLARE @@instanceViewSql9 NVARCHAR(4000)
    DECLARE @@instanceViewSql10 NVARCHAR(4000)
    DECLARE @@instanceViewSql11 NVARCHAR(4000)
    
    DECLARE @@relationshipViewSql NVARCHAR(4000)
    DECLARE @@relationshipViewSql1 NVARCHAR(4000)
    DECLARE @@relationshipViewSql2 NVARCHAR(4000)
    DECLARE @@relationshipViewSql3 NVARCHAR(4000)
    DECLARE @@relationshipViewSql4 NVARCHAR(4000)
    DECLARE @@relationshipViewSql5 NVARCHAR(4000)
    DECLARE @@relationshipViewSql6 NVARCHAR(4000)
    DECLARE @@relationshipViewSql7 NVARCHAR(4000)
    DECLARE @@relationshipViewSql8 NVARCHAR(4000)
    DECLARE @@relationshipViewSql9 NVARCHAR(4000)
    DECLARE @@relationshipViewSql10 NVARCHAR(4000)
    DECLARE @@relationshipViewSql11 NVARCHAR(4000)
    
    -- Initialize string buffers
    SET @@instanceViewSql  = ''
    SET @@instanceViewSql1 = ''
    SET @@instanceViewSql2 = ''
    SET @@instanceViewSql3 = ''
    SET @@instanceViewSql4 = ''
    SET @@instanceViewSql5 = ''
    SET @@instanceViewSql6 = ''
    SET @@instanceViewSql7 = ''
    SET @@instanceViewSql8 = ''
    SET @@instanceViewSql9 = ''
    SET @@instanceViewSql10 = ''
    SET @@instanceViewSql11 = ''
    
    SET @@relationshipViewSql  = ''
    SET @@relationshipViewSql1 = ''
    SET @@relationshipViewSql2 = ''
    SET @@relationshipViewSql3 = ''
    SET @@relationshipViewSql4 = ''
    SET @@relationshipViewSql5 = ''
    SET @@relationshipViewSql6 = ''
    SET @@relationshipViewSql7 = ''
    SET @@relationshipViewSql8 = ''
    SET @@relationshipViewSql9 = ''
    SET @@relationshipViewSql10 = ''
    SET @@relationshipViewSql11 = ''

    IF (@isArchivingView = 0)
    BEGIN
        DECLARE partition_cursor CURSOR LOCAL FOR 
            SELECT InstancesTable, RelationshipsTable 
            FROM [dbo].[bam_Metadata_Partitions] 
            WHERE ActivityName = @activityName AND ArchivedTime IS NULL
            ORDER BY CreationTime ASC
            
        -- Initialization
        SET @@instanceViewSql = 'ALTER VIEW dbo.[bam_' + @activityName  + '_CompletedInstances' + '] AS '
            + ' SELECT * FROM dbo.[bam_' + @activityName + '_Completed] WITH (NOLOCK)'
            
        SET @@relationshipViewSql = 'ALTER VIEW dbo.[bam_' + @activityName  + '_AllRelationships]' + ' AS ' 
            + ' SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'''') AS ReferenceData, LongReferenceData FROM [bam_' + @activityName + '_ActiveRelationships] WITH (NOLOCK)' 
            + ' UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'''') AS ReferenceData, LongReferenceData FROM dbo.[bam_' + @activityName + '_CompletedRelationships] WITH (NOLOCK)'
    END
    ELSE
    BEGIN
        DECLARE partition_cursor CURSOR LOCAL FOR 
            SELECT InstancesTable, RelationshipsTable 
            FROM [dbo].[bam_Metadata_Partitions]
            WHERE ActivityName = @activityName    
                AND ArchivedTime IS NULL                        -- hasn't been archived
                AND CreationTime < @partitionOnlineThreshold -- falling out of online window        
                AND (@analysisRecordWatermark IS NULL
                    OR MaxRecordID <= @analysisRecordWatermark)
            ORDER BY CreationTime ASC

        -- Initialization
        SET @@instanceViewSql = 'ALTER VIEW dbo.[bam_' + @activityName + '_InstancesForArchive] AS '
        SET @@relationshipViewSql = 'ALTER VIEW dbo.[bam_' + @activityName + '_RelationshipsForArchive] AS '
    END
      
    DECLARE @@bufferCount INT        -- sql script buffer counter
    SET @@bufferCount = 0

    DECLARE @@firstPartition BIT
    SET @@firstPartition = 1
    
    DECLARE @@instancePartition sysname
    DECLARE @@relationshipPartition sysname
    
    DECLARE @@unionInstancePartition NVARCHAR(4000)
    DECLARE @@unionRelationshipPartition NVARCHAR(4000)
    
    OPEN partition_cursor
    FETCH NEXT FROM partition_cursor INTO @@instancePartition, @@relationshipPartition
    WHILE @@fetch_status = 0
    BEGIN
        IF @isArchivingView = 0
            BEGIN
                -- Build union sub-statements
                SET @@unionInstancePartition = ' UNION ALL SELECT * FROM [dbo].[' + @@instancePartition + '] WITH (NOLOCK)'
                SET @@unionRelationshipPartition = ' UNION ALL SELECT ActivityID, ReferenceType, ReferenceName, ReferenceData+ISNULL(ReferenceDataExtend, N'''') AS ReferenceData, LongReferenceData FROM [dbo].[' 
                    + @@relationshipPartition + '] WITH (NOLOCK)'
            END
        ELSE
            BEGIN
                IF @@firstPartition = 1
                BEGIN
                    SET @@unionInstancePartition = 'SELECT * FROM [dbo].[' + @@instancePartition + '] WITH (NOLOCK)'
                    SET @@unionRelationshipPartition = 'SELECT * FROM [dbo].[' + @@relationshipPartition + '] WITH (NOLOCK)'
                    SET @@firstPartition = 0
                END
                ELSE
                BEGIN        
                    SET @@unionInstancePartition = ' UNION ALL SELECT * FROM [dbo].[' + @@instancePartition + '] WITH (NOLOCK)'
                    SET @@unionRelationshipPartition = ' UNION ALL SELECT * FROM [dbo].[' + @@relationshipPartition + '] WITH (NOLOCK)'
                END            
            END    
        
        IF (@@bufferCount = 0)
        BEGIN
            IF LEN(@@relationshipViewSql) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql = @@instanceViewSql + @@unionInstancePartition
                SET @@relationshipViewSql = @@relationshipViewSql + @@unionRelationshipPartition
            END
            ELSE
                SET @@bufferCount = 1
        END

        IF (@@bufferCount = 1)
        BEGIN
            IF LEN(@@relationshipViewSql1) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql1 = @@instanceViewSql1 + @@unionInstancePartition
                SET @@relationshipViewSql1 = @@relationshipViewSql1 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 2
        END

        IF (@@bufferCount = 2)
        BEGIN
            IF LEN(@@relationshipViewSql2) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql2 = @@instanceViewSql2 + @@unionInstancePartition
                SET @@relationshipViewSql2 = @@relationshipViewSql2 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 3
        END

        IF (@@bufferCount = 3)
        BEGIN
            IF LEN(@@relationshipViewSql3) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql3 = @@instanceViewSql3 + @@unionInstancePartition
                SET @@relationshipViewSql3 = @@relationshipViewSql3 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 4
        END

        IF (@@bufferCount = 4)
        BEGIN
            IF LEN(@@relationshipViewSql4) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql4 = @@instanceViewSql4 + @@unionInstancePartition
                SET @@relationshipViewSql4 = @@relationshipViewSql4 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 5
        END

        IF (@@bufferCount = 5)
        BEGIN
            IF LEN(@@relationshipViewSql5) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql5 = @@instanceViewSql5 + @@unionInstancePartition
                SET @@relationshipViewSql5 = @@relationshipViewSql5 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 6
        END

        IF (@@bufferCount = 6)
        BEGIN
            IF LEN(@@relationshipViewSql6) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql6 = @@instanceViewSql6 + @@unionInstancePartition
                SET @@relationshipViewSql6 = @@relationshipViewSql6 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 7
        END

        IF (@@bufferCount = 7)
        BEGIN
            IF LEN(@@relationshipViewSql7) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql7 = @@instanceViewSql7 + @@unionInstancePartition
                SET @@relationshipViewSql7 = @@relationshipViewSql7 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 8
        END

        IF (@@bufferCount = 8)
        BEGIN
            IF LEN(@@relationshipViewSql8) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql8 = @@instanceViewSql8 + @@unionInstancePartition
                SET @@relationshipViewSql8 = @@relationshipViewSql8 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 9
        END

        IF (@@bufferCount = 9)
        BEGIN
            IF LEN(@@relationshipViewSql9) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql9 = @@instanceViewSql9 + @@unionInstancePartition
                SET @@relationshipViewSql9 = @@relationshipViewSql9 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 10
        END

        IF (@@bufferCount = 10)
        BEGIN
            IF LEN(@@relationshipViewSql10) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql10 = @@instanceViewSql10 + @@unionInstancePartition
                SET @@relationshipViewSql10 = @@relationshipViewSql10 + @@unionRelationshipPartition
            END
            ELSE 
                SET @@bufferCount = 11
        END

        IF (@@bufferCount = 11)
        BEGIN
            IF LEN(@@relationshipViewSql11) + LEN(@@unionRelationshipPartition) <= 4000
            BEGIN
                SET @@instanceViewSql11 = @@instanceViewSql11 + @@unionInstancePartition
                SET @@relationshipViewSql11 = @@relationshipViewSql11 + @@unionRelationshipPartition
            END
            ELSE 
            BEGIN
                RAISERROR (N'RegenerateViews_ViewDefinitionTooLong', 16, 1)
                RETURN
            END
        END

        FETCH NEXT FROM partition_cursor INTO @@instancePartition, @@relationshipPartition
    END
    
    CLOSE partition_cursor
    DEALLOCATE partition_cursor

    -- Recreate the instances and relationships views
    EXEC (@@instanceViewSql + @@instanceViewSql1 + @@instanceViewSql2
        + @@instanceViewSql3 + @@instanceViewSql4 + @@instanceViewSql5 
        + @@instanceViewSql6 + @@instanceViewSql7 + @@instanceViewSql8 
        + @@instanceViewSql9 + @@instanceViewSql10 + @@instanceViewSql11)
    
    IF (@@ERROR <> 0) RETURN 255
        
    PRINT 'Instance view recreated successfully.'
    
    EXEC (@@relationshipViewSql + @@relationshipViewSql1 + @@relationshipViewSql2
        + @@relationshipViewSql3 + @@relationshipViewSql4 + @@relationshipViewSql5 
        + @@relationshipViewSql6 + @@relationshipViewSql7 + @@relationshipViewSql8 
        + @@relationshipViewSql9 + @@relationshipViewSql10 + @@relationshipViewSql11)

    IF (@@ERROR <> 0) RETURN 255
                
    PRINT 'Relationship view recreated successfully.'

    RETURN 0