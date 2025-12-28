CREATE PROCEDURE [dbo].[bam_EmailFromFile_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@FileReceived DATETIME  = NULL,
            
            @@FileContent3885 NVARCHAR (3885) = NULL,
            
            @@EmailConstructed DATETIME  = NULL,
            
            @@EmailServer NVARCHAR (50) = NULL,
            
            @@EmailRecipient NVARCHAR (256) = NULL,
            
            @@EmailSender NVARCHAR (256) = NULL,
            
            @@EmaiRecipientAdded DATETIME  = NULL,
            
            @@EmailSent DATETIME  = NULL
            
        
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
                FROM [dbo].[bam_EmailFromFile_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_EmailFromFile_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_EmailFromFile_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [FileReceived],
          [FileContent3885],
          [EmailConstructed],
          [EmailServer],
          [EmailRecipient],
          [EmailSender],
          [EmaiRecipientAdded],
          [EmailSent]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@FileReceived, primaryImport.[FileReceived]),
                    COALESCE(@@FileContent3885, primaryImport.[FileContent3885]),
                    COALESCE(@@EmailConstructed, primaryImport.[EmailConstructed]),
                    COALESCE(@@EmailServer, primaryImport.[EmailServer]),
                    COALESCE(@@EmailRecipient, primaryImport.[EmailRecipient]),
                    COALESCE(@@EmailSender, primaryImport.[EmailSender]),
                    COALESCE(@@EmaiRecipientAdded, primaryImport.[EmaiRecipientAdded]),
                    COALESCE(@@EmailSent, primaryImport.[EmailSent])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_EmailFromFile_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'EmailFromFile'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_EmailFromFile_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_EmailFromFile_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_EmailFromFile_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_EmailFromFile_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_EmailFromFile_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [FileReceived] = COALESCE(@@FileReceived, primaryImport.[FileReceived]),
                [FileContent3885] = COALESCE(@@FileContent3885, primaryImport.[FileContent3885]),
                [EmailConstructed] = COALESCE(@@EmailConstructed, primaryImport.[EmailConstructed]),
                [EmailServer] = COALESCE(@@EmailServer, primaryImport.[EmailServer]),
                [EmailRecipient] = COALESCE(@@EmailRecipient, primaryImport.[EmailRecipient]),
                [EmailSender] = COALESCE(@@EmailSender, primaryImport.[EmailSender]),
                [EmaiRecipientAdded] = COALESCE(@@EmaiRecipientAdded, primaryImport.[EmaiRecipientAdded]),
                [EmailSent] = COALESCE(@@EmailSent, primaryImport.[EmailSent])    
            FROM [dbo].[bam_EmailFromFile_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_EmailFromFile_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [FileReceived],
          [FileContent3885],
          [EmailConstructed],
          [EmailServer],
          [EmailRecipient],
          [EmailSender],
          [EmaiRecipientAdded],
          [EmailSent]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@FileReceived,
                    @@FileContent3885,
                    @@EmailConstructed,
                    @@EmailServer,
                    @@EmailRecipient,
                    @@EmailSender,
                    @@EmaiRecipientAdded,
                    @@EmailSent    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  