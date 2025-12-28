CREATE PROCEDURE [dbo].[bam_ExpiredCreditCards_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@ID_CreditCards NVARCHAR (50) = NULL,
            
            @@ID_UserObjects NVARCHAR (50) = NULL,
            
            @@DateCreated DATETIME  = NULL,
            
            @@Last4Digits NVARCHAR (4) = NULL,
            
            @@ExpirationMonth NVARCHAR (2) = NULL,
            
            @@ExpirationYear NVARCHAR (4) = NULL,
            
            @@Alias NVARCHAR (50) = NULL,
            
            @@EmailAddress NVARCHAR (50) = NULL
            
        
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
                FROM [dbo].[bam_ExpiredCreditCards_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_ExpiredCreditCards_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_ExpiredCreditCards_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [ID_CreditCards],
          [ID_UserObjects],
          [DateCreated],
          [Last4Digits],
          [ExpirationMonth],
          [ExpirationYear],
          [Alias],
          [EmailAddress]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@ID_CreditCards, primaryImport.[ID_CreditCards]),
                    COALESCE(@@ID_UserObjects, primaryImport.[ID_UserObjects]),
                    COALESCE(@@DateCreated, primaryImport.[DateCreated]),
                    COALESCE(@@Last4Digits, primaryImport.[Last4Digits]),
                    COALESCE(@@ExpirationMonth, primaryImport.[ExpirationMonth]),
                    COALESCE(@@ExpirationYear, primaryImport.[ExpirationYear]),
                    COALESCE(@@Alias, primaryImport.[Alias]),
                    COALESCE(@@EmailAddress, primaryImport.[EmailAddress])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_ExpiredCreditCards_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'ExpiredCreditCards'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_ExpiredCreditCards_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_ExpiredCreditCards_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_ExpiredCreditCards_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_ExpiredCreditCards_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_ExpiredCreditCards_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [ID_CreditCards] = COALESCE(@@ID_CreditCards, primaryImport.[ID_CreditCards]),
                [ID_UserObjects] = COALESCE(@@ID_UserObjects, primaryImport.[ID_UserObjects]),
                [DateCreated] = COALESCE(@@DateCreated, primaryImport.[DateCreated]),
                [Last4Digits] = COALESCE(@@Last4Digits, primaryImport.[Last4Digits]),
                [ExpirationMonth] = COALESCE(@@ExpirationMonth, primaryImport.[ExpirationMonth]),
                [ExpirationYear] = COALESCE(@@ExpirationYear, primaryImport.[ExpirationYear]),
                [Alias] = COALESCE(@@Alias, primaryImport.[Alias]),
                [EmailAddress] = COALESCE(@@EmailAddress, primaryImport.[EmailAddress])    
            FROM [dbo].[bam_ExpiredCreditCards_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_ExpiredCreditCards_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [ID_CreditCards],
          [ID_UserObjects],
          [DateCreated],
          [Last4Digits],
          [ExpirationMonth],
          [ExpirationYear],
          [Alias],
          [EmailAddress]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@ID_CreditCards,
                    @@ID_UserObjects,
                    @@DateCreated,
                    @@Last4Digits,
                    @@ExpirationMonth,
                    @@ExpirationYear,
                    @@Alias,
                    @@EmailAddress    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  