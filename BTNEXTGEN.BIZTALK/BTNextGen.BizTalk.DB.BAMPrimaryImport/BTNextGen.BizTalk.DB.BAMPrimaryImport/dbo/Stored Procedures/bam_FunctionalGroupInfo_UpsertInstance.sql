CREATE PROCEDURE [dbo].[bam_FunctionalGroupInfo_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@InterchangeActivityID NVARCHAR (128) = NULL,
            
            @@GroupControlNo NVARCHAR (14) = NULL,
            
            @@FunctionalIDCode NVARCHAR (6) = NULL,
            
            @@TSCount INT  = NULL
            
        
      )
    
    AS
    BEGIN
      
            IF (@@ActivityID is NULL)
            BEGIN
                RETURN 0
            END

            -- Find which row we are contributing to
            DECLARE @@HasContinuations BIT

            -- If main trace is not currently marked as completed, 
            -- check if it has already been marked as completed before
            IF (ISNULL(@@IsComplete, 0) = 0)
            BEGIN
                SELECT @@IsComplete = ISNULL(IsComplete, 0) 
                FROM [dbo].[bam_FunctionalGroupInfo_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_FunctionalGroupInfo_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_FunctionalGroupInfo_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [InterchangeActivityID],
          [GroupControlNo],
          [FunctionalIDCode],
          [TSCount]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@InterchangeActivityID, primaryImport.[InterchangeActivityID]),
                    COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                    COALESCE(@@FunctionalIDCode, primaryImport.[FunctionalIDCode]),
                    COALESCE(@@TSCount, primaryImport.[TSCount])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_FunctionalGroupInfo_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'FunctionalGroupInfo'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_FunctionalGroupInfo_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_FunctionalGroupInfo_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_FunctionalGroupInfo_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_FunctionalGroupInfo_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_FunctionalGroupInfo_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [InterchangeActivityID] = COALESCE(@@InterchangeActivityID, primaryImport.[InterchangeActivityID]),
                [GroupControlNo] = COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                [FunctionalIDCode] = COALESCE(@@FunctionalIDCode, primaryImport.[FunctionalIDCode]),
                [TSCount] = COALESCE(@@TSCount, primaryImport.[TSCount])    
            FROM [dbo].[bam_FunctionalGroupInfo_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_FunctionalGroupInfo_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [InterchangeActivityID],
          [GroupControlNo],
          [FunctionalIDCode],
          [TSCount]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@InterchangeActivityID,
                    @@GroupControlNo,
                    @@FunctionalIDCode,
                    @@TSCount    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  