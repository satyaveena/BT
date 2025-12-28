CREATE PROCEDURE [dbo].[bam_BusinessMessageJournal_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@MessageTrackingID NVARCHAR (128) = NULL,
            
            @@ActionType INT  = NULL,
            
            @@ContainerActivityID NVARCHAR (128) = NULL,
            
            @@ContainerType INT  = NULL,
            
            @@BTSInterchangeID NVARCHAR (128) = NULL,
            
            @@BTSMessageID NVARCHAR (128) = NULL,
            
            @@BTSServiceInstanceID NVARCHAR (128) = NULL,
            
            @@BTSHostName NVARCHAR (128) = NULL,
            
            @@RoutedToPartyName NVARCHAR (256) = NULL,
            
            @@LinkedMessageTrackingID NVARCHAR (128) = NULL,
            
            @@TimeCreated DATETIME  = NULL
            
        
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
                FROM [dbo].[bam_BusinessMessageJournal_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_BusinessMessageJournal_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_BusinessMessageJournal_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [MessageTrackingID],
          [ActionType],
          [ContainerActivityID],
          [ContainerType],
          [BTSInterchangeID],
          [BTSMessageID],
          [BTSServiceInstanceID],
          [BTSHostName],
          [RoutedToPartyName],
          [LinkedMessageTrackingID],
          [TimeCreated]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@MessageTrackingID, primaryImport.[MessageTrackingID]),
                    COALESCE(@@ActionType, primaryImport.[ActionType]),
                    COALESCE(@@ContainerActivityID, primaryImport.[ContainerActivityID]),
                    COALESCE(@@ContainerType, primaryImport.[ContainerType]),
                    COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID]),
                    COALESCE(@@BTSMessageID, primaryImport.[BTSMessageID]),
                    COALESCE(@@BTSServiceInstanceID, primaryImport.[BTSServiceInstanceID]),
                    COALESCE(@@BTSHostName, primaryImport.[BTSHostName]),
                    COALESCE(@@RoutedToPartyName, primaryImport.[RoutedToPartyName]),
                    COALESCE(@@LinkedMessageTrackingID, primaryImport.[LinkedMessageTrackingID]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_BusinessMessageJournal_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'BusinessMessageJournal'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_BusinessMessageJournal_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_BusinessMessageJournal_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_BusinessMessageJournal_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_BusinessMessageJournal_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_BusinessMessageJournal_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [MessageTrackingID] = COALESCE(@@MessageTrackingID, primaryImport.[MessageTrackingID]),
                [ActionType] = COALESCE(@@ActionType, primaryImport.[ActionType]),
                [ContainerActivityID] = COALESCE(@@ContainerActivityID, primaryImport.[ContainerActivityID]),
                [ContainerType] = COALESCE(@@ContainerType, primaryImport.[ContainerType]),
                [BTSInterchangeID] = COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID]),
                [BTSMessageID] = COALESCE(@@BTSMessageID, primaryImport.[BTSMessageID]),
                [BTSServiceInstanceID] = COALESCE(@@BTSServiceInstanceID, primaryImport.[BTSServiceInstanceID]),
                [BTSHostName] = COALESCE(@@BTSHostName, primaryImport.[BTSHostName]),
                [RoutedToPartyName] = COALESCE(@@RoutedToPartyName, primaryImport.[RoutedToPartyName]),
                [LinkedMessageTrackingID] = COALESCE(@@LinkedMessageTrackingID, primaryImport.[LinkedMessageTrackingID]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated])    
            FROM [dbo].[bam_BusinessMessageJournal_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_BusinessMessageJournal_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [MessageTrackingID],
          [ActionType],
          [ContainerActivityID],
          [ContainerType],
          [BTSInterchangeID],
          [BTSMessageID],
          [BTSServiceInstanceID],
          [BTSHostName],
          [RoutedToPartyName],
          [LinkedMessageTrackingID],
          [TimeCreated]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@MessageTrackingID,
                    @@ActionType,
                    @@ContainerActivityID,
                    @@ContainerType,
                    @@BTSInterchangeID,
                    @@BTSMessageID,
                    @@BTSServiceInstanceID,
                    @@BTSHostName,
                    @@RoutedToPartyName,
                    @@LinkedMessageTrackingID,
                    @@TimeCreated    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  