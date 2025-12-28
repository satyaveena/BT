CREATE PROCEDURE [dbo].[bam_BatchInterchangeActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@InterchangeControlNo NVARCHAR (14) = NULL,
            
            @@ReceiverPartyName NVARCHAR (256) = NULL,
            
            @@SenderPartyName NVARCHAR (256) = NULL,
            
            @@ReceiverID NVARCHAR (35) = NULL,
            
            @@SenderID NVARCHAR (35) = NULL,
            
            @@ReceiverQ NVARCHAR (4) = NULL,
            
            @@SenderQ NVARCHAR (4) = NULL,
            
            @@InterchangeDateTime DATETIME  = NULL,
            
            @@Direction INT  = NULL,
            
            @@TimeCreated DATETIME  = NULL,
            
            @@RowFlags INT  = NULL,
            
            @@BatchCorrelationID NVARCHAR (40) = NULL,
            
            @@AgreementName NVARCHAR (256) = NULL
            
        
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
                FROM [dbo].[bam_BatchInterchangeActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_BatchInterchangeActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_BatchInterchangeActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [InterchangeControlNo],
          [ReceiverPartyName],
          [SenderPartyName],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [TimeCreated],
          [RowFlags],
          [BatchCorrelationID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                    COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                    COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                    COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                    COALESCE(@@SenderID, primaryImport.[SenderID]),
                    COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                    COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                    COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                    COALESCE(@@Direction, primaryImport.[Direction]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@BatchCorrelationID, primaryImport.[BatchCorrelationID]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_BatchInterchangeActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'BatchInterchangeActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_BatchInterchangeActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_BatchInterchangeActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_BatchInterchangeActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_BatchInterchangeActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_BatchInterchangeActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [InterchangeControlNo] = COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                [ReceiverPartyName] = COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                [SenderPartyName] = COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                [ReceiverID] = COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                [SenderID] = COALESCE(@@SenderID, primaryImport.[SenderID]),
                [ReceiverQ] = COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                [SenderQ] = COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                [InterchangeDateTime] = COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                [Direction] = COALESCE(@@Direction, primaryImport.[Direction]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [BatchCorrelationID] = COALESCE(@@BatchCorrelationID, primaryImport.[BatchCorrelationID]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_BatchInterchangeActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_BatchInterchangeActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [InterchangeControlNo],
          [ReceiverPartyName],
          [SenderPartyName],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [TimeCreated],
          [RowFlags],
          [BatchCorrelationID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@InterchangeControlNo,
                    @@ReceiverPartyName,
                    @@SenderPartyName,
                    @@ReceiverID,
                    @@SenderID,
                    @@ReceiverQ,
                    @@SenderQ,
                    @@InterchangeDateTime,
                    @@Direction,
                    @@TimeCreated,
                    @@RowFlags,
                    @@BatchCorrelationID,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  