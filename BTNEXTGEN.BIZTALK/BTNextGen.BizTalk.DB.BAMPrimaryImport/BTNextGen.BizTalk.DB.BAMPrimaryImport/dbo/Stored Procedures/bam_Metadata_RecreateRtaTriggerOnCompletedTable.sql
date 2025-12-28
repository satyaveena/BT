-- Called by bam_Metadata_SpawnPartition
CREATE PROCEDURE [dbo].[bam_Metadata_RecreateRtaTriggerOnCompletedTable]
(
    @activityName NVARCHAR(256),
    @rtaTriggerName sysname
)
AS
    -- Validate activity name and RTA trigger name
    IF @activityName IS NULL OR @rtaTriggerName IS NULL
        OR LEN(@activityName) = 0 OR LEN(@rtaTriggerName) = 0
        RETURN
        
    DECLARE @@rtaTriggerDef NVARCHAR(4000)
    DECLARE @@rtaTriggerDef1 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef2 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef3 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef4 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef5 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef6 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef7 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef8 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef9 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef10 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef11 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef12 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef13 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef14 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef15 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef16 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef17 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef18 NVARCHAR(4000)
    DECLARE @@rtaTriggerDef19 NVARCHAR(4000)
    
    SET @@rtaTriggerDef1 = ''
    SET @@rtaTriggerDef2 = ''
    SET @@rtaTriggerDef3 = ''
    SET @@rtaTriggerDef4 = ''
    SET @@rtaTriggerDef5 = ''
    SET @@rtaTriggerDef6 = ''
    SET @@rtaTriggerDef7 = ''
    SET @@rtaTriggerDef8 = ''
    SET @@rtaTriggerDef9 = ''
    SET @@rtaTriggerDef10 = ''
    SET @@rtaTriggerDef11 = ''
    SET @@rtaTriggerDef12 = ''
    SET @@rtaTriggerDef13 = ''
    SET @@rtaTriggerDef14 = ''
    SET @@rtaTriggerDef15 = ''
    SET @@rtaTriggerDef16 = ''
    SET @@rtaTriggerDef17 = ''
    SET @@rtaTriggerDef18 = ''
    SET @@rtaTriggerDef19 = ''

    DECLARE @@commentCount INT 
    SET @@commentCount = 1

    DECLARE comments CURSOR local 
    FOR SELECT [text] from syscomments where id = OBJECT_ID('dbo.' + @rtaTriggerName)

    OPEN comments
    FETCH NEXT FROM comments INTO @@rtaTriggerDef
    WHILE @@fetch_status = 0 
    BEGIN
        IF @@commentCount = 1
            SET @@rtaTriggerDef1 = @@rtaTriggerDef
        ELSE IF @@commentCount = 2
            SET @@rtaTriggerDef2 = @@rtaTriggerDef
        ELSE IF @@commentCount = 3
            SET @@rtaTriggerDef3 = @@rtaTriggerDef
        ELSE IF @@commentCount = 4
            SET @@rtaTriggerDef4 = @@rtaTriggerDef
        ELSE IF @@commentCount = 5
            SET @@rtaTriggerDef5 = @@rtaTriggerDef
        ELSE IF @@commentCount = 6
            SET @@rtaTriggerDef6 = @@rtaTriggerDef
        ELSE IF @@commentCount = 7
            SET @@rtaTriggerDef7 = @@rtaTriggerDef
        ELSE IF @@commentCount = 8
            SET @@rtaTriggerDef8 = @@rtaTriggerDef
        ELSE IF @@commentCount = 9
            SET @@rtaTriggerDef9 = @@rtaTriggerDef
        ELSE IF @@commentCount = 10
            SET @@rtaTriggerDef10 = @@rtaTriggerDef
        ELSE IF @@commentCount = 11
            SET @@rtaTriggerDef11 = @@rtaTriggerDef
        ELSE IF @@commentCount = 12
            SET @@rtaTriggerDef12 = @@rtaTriggerDef
        ELSE IF @@commentCount = 13
            SET @@rtaTriggerDef13 = @@rtaTriggerDef
        ELSE IF @@commentCount = 14
            SET @@rtaTriggerDef14 = @@rtaTriggerDef
        ELSE IF @@commentCount = 15
            SET @@rtaTriggerDef15 = @@rtaTriggerDef
        ELSE IF @@commentCount = 16
            SET @@rtaTriggerDef16 = @@rtaTriggerDef
        ELSE IF @@commentCount = 17
            SET @@rtaTriggerDef17 = @@rtaTriggerDef
        ELSE IF @@commentCount = 18
            SET @@rtaTriggerDef18 = @@rtaTriggerDef
        ELSE IF @@commentCount = 19
            SET @@rtaTriggerDef19 = @@rtaTriggerDef

        SET @@commentCount = @@commentCount + 1
        FETCH NEXT FROM comments INTO @@rtaTriggerDef
    END
    CLOSE comments
    DEALLOCATE comments

    -- If RTA trigger not found, return
    IF LEN(@@rtaTriggerDef1) = 0 
        RETURN 0
        
    -- Drop the trigger and recreate it on the new completed instances table
    EXEC ('DROP TRIGGER [' + @rtaTriggerName + ']')
    IF (@@ERROR <> 0) RETURN 255
    
    EXEC (@@rtaTriggerDef1 + @@rtaTriggerDef2 + @@rtaTriggerDef3 + @@rtaTriggerDef4 + @@rtaTriggerDef5
            + @@rtaTriggerDef6 + @@rtaTriggerDef7 + @@rtaTriggerDef8 + @@rtaTriggerDef9 + @@rtaTriggerDef10
            + @@rtaTriggerDef11 + @@rtaTriggerDef12 + @@rtaTriggerDef13 + @@rtaTriggerDef14 + @@rtaTriggerDef15
            + @@rtaTriggerDef16 + @@rtaTriggerDef17 + @@rtaTriggerDef18 + @@rtaTriggerDef19)
    IF (@@ERROR <> 0) RETURN 255