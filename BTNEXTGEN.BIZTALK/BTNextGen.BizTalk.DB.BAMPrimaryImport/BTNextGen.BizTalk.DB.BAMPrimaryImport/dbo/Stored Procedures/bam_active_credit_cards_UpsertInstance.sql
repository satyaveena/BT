CREATE PROCEDURE [dbo].[bam_active_credit_cards_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@CSCardRcv DATETIME  = NULL,
            
            @@CardID NVARCHAR (50) = NULL,
            
            @@ERPAccountNo NVARCHAR (50) = NULL,
            
            @@Destination NVARCHAR (50) = NULL,
            
            @@Alias NVARCHAR (150) = NULL,
            
            @@ERPSent DATETIME  = NULL,
            
            @@CSAckSent DATETIME  = NULL
            
        
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
                FROM [dbo].[bam_active_credit_cards_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_active_credit_cards_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_active_credit_cards_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [CSCardRcv],
          [CardID],
          [ERPAccountNo],
          [Destination],
          [Alias],
          [ERPSent],
          [CSAckSent]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@CSCardRcv, primaryImport.[CSCardRcv]),
                    COALESCE(@@CardID, primaryImport.[CardID]),
                    COALESCE(@@ERPAccountNo, primaryImport.[ERPAccountNo]),
                    COALESCE(@@Destination, primaryImport.[Destination]),
                    COALESCE(@@Alias, primaryImport.[Alias]),
                    COALESCE(@@ERPSent, primaryImport.[ERPSent]),
                    COALESCE(@@CSAckSent, primaryImport.[CSAckSent])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_active_credit_cards_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'active_credit_cards'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_active_credit_cards_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_active_credit_cards_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_active_credit_cards_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_active_credit_cards_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_active_credit_cards_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [CSCardRcv] = COALESCE(@@CSCardRcv, primaryImport.[CSCardRcv]),
                [CardID] = COALESCE(@@CardID, primaryImport.[CardID]),
                [ERPAccountNo] = COALESCE(@@ERPAccountNo, primaryImport.[ERPAccountNo]),
                [Destination] = COALESCE(@@Destination, primaryImport.[Destination]),
                [Alias] = COALESCE(@@Alias, primaryImport.[Alias]),
                [ERPSent] = COALESCE(@@ERPSent, primaryImport.[ERPSent]),
                [CSAckSent] = COALESCE(@@CSAckSent, primaryImport.[CSAckSent])    
            FROM [dbo].[bam_active_credit_cards_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_active_credit_cards_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [CSCardRcv],
          [CardID],
          [ERPAccountNo],
          [Destination],
          [Alias],
          [ERPSent],
          [CSAckSent]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@CSCardRcv,
                    @@CardID,
                    @@ERPAccountNo,
                    @@Destination,
                    @@Alias,
                    @@ERPSent,
                    @@CSAckSent    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  