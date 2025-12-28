CREATE PROCEDURE [dbo].[bam_AS2MessageActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@ReceiverPartyName NVARCHAR (256) = NULL,
            
            @@SenderPartyName NVARCHAR (256) = NULL,
            
            @@AS2PartyRole INT  = NULL,
            
            @@AS2From NVARCHAR (128) = NULL,
            
            @@AS2To NVARCHAR (128) = NULL,
            
            @@MessageID NVARCHAR (1000) = NULL,
            
            @@MessageDateTime DATETIME  = NULL,
            
            @@BTSInterchangeID NVARCHAR (128) = NULL,
            
            @@BTSMessageID NVARCHAR (128) = NULL,
            
            @@MdnProcessingStatus INT  = NULL,
            
            @@MessageEncryptionType INT  = NULL,
            
            @@IsMdnExpected INT  = NULL,
            
            @@MicAlgorithmType INT  = NULL,
            
            @@MessageSignatureType INT  = NULL,
            
            @@MessagePayloadContentKey NVARCHAR (40) = NULL,
            
            @@MessageWireContentKey NVARCHAR (40) = NULL,
            
            @@MessageMicValue NVARCHAR (50) = NULL,
            
            @@TimeCreated DATETIME  = NULL,
            
            @@RowFlags INT  = NULL,
            
            @@IsAS2MessageDuplicate INT  = NULL,
            
            @@DaysToCheckDuplicate INT  = NULL,
            
            @@Filename NVARCHAR (1000) = NULL,
            
            @@TrackingActivityID NVARCHAR (128) = NULL,
            
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
                FROM [dbo].[bam_AS2MessageActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_AS2MessageActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_AS2MessageActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [ReceiverPartyName],
          [SenderPartyName],
          [AS2PartyRole],
          [AS2From],
          [AS2To],
          [MessageID],
          [MessageDateTime],
          [BTSInterchangeID],
          [BTSMessageID],
          [MdnProcessingStatus],
          [MessageEncryptionType],
          [IsMdnExpected],
          [MicAlgorithmType],
          [MessageSignatureType],
          [MessagePayloadContentKey],
          [MessageWireContentKey],
          [MessageMicValue],
          [TimeCreated],
          [RowFlags],
          [IsAS2MessageDuplicate],
          [DaysToCheckDuplicate],
          [Filename],
          [TrackingActivityID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                    COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                    COALESCE(@@AS2PartyRole, primaryImport.[AS2PartyRole]),
                    COALESCE(@@AS2From, primaryImport.[AS2From]),
                    COALESCE(@@AS2To, primaryImport.[AS2To]),
                    COALESCE(@@MessageID, primaryImport.[MessageID]),
                    COALESCE(@@MessageDateTime, primaryImport.[MessageDateTime]),
                    COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID]),
                    COALESCE(@@BTSMessageID, primaryImport.[BTSMessageID]),
                    COALESCE(@@MdnProcessingStatus, primaryImport.[MdnProcessingStatus]),
                    COALESCE(@@MessageEncryptionType, primaryImport.[MessageEncryptionType]),
                    COALESCE(@@IsMdnExpected, primaryImport.[IsMdnExpected]),
                    COALESCE(@@MicAlgorithmType, primaryImport.[MicAlgorithmType]),
                    COALESCE(@@MessageSignatureType, primaryImport.[MessageSignatureType]),
                    COALESCE(@@MessagePayloadContentKey, primaryImport.[MessagePayloadContentKey]),
                    COALESCE(@@MessageWireContentKey, primaryImport.[MessageWireContentKey]),
                    COALESCE(@@MessageMicValue, primaryImport.[MessageMicValue]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@IsAS2MessageDuplicate, primaryImport.[IsAS2MessageDuplicate]),
                    COALESCE(@@DaysToCheckDuplicate, primaryImport.[DaysToCheckDuplicate]),
                    COALESCE(@@Filename, primaryImport.[Filename]),
                    COALESCE(@@TrackingActivityID, primaryImport.[TrackingActivityID]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_AS2MessageActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'AS2MessageActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_AS2MessageActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_AS2MessageActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_AS2MessageActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_AS2MessageActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_AS2MessageActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [ReceiverPartyName] = COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                [SenderPartyName] = COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                [AS2PartyRole] = COALESCE(@@AS2PartyRole, primaryImport.[AS2PartyRole]),
                [AS2From] = COALESCE(@@AS2From, primaryImport.[AS2From]),
                [AS2To] = COALESCE(@@AS2To, primaryImport.[AS2To]),
                [MessageID] = COALESCE(@@MessageID, primaryImport.[MessageID]),
                [MessageDateTime] = COALESCE(@@MessageDateTime, primaryImport.[MessageDateTime]),
                [BTSInterchangeID] = COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID]),
                [BTSMessageID] = COALESCE(@@BTSMessageID, primaryImport.[BTSMessageID]),
                [MdnProcessingStatus] = COALESCE(@@MdnProcessingStatus, primaryImport.[MdnProcessingStatus]),
                [MessageEncryptionType] = COALESCE(@@MessageEncryptionType, primaryImport.[MessageEncryptionType]),
                [IsMdnExpected] = COALESCE(@@IsMdnExpected, primaryImport.[IsMdnExpected]),
                [MicAlgorithmType] = COALESCE(@@MicAlgorithmType, primaryImport.[MicAlgorithmType]),
                [MessageSignatureType] = COALESCE(@@MessageSignatureType, primaryImport.[MessageSignatureType]),
                [MessagePayloadContentKey] = COALESCE(@@MessagePayloadContentKey, primaryImport.[MessagePayloadContentKey]),
                [MessageWireContentKey] = COALESCE(@@MessageWireContentKey, primaryImport.[MessageWireContentKey]),
                [MessageMicValue] = COALESCE(@@MessageMicValue, primaryImport.[MessageMicValue]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [IsAS2MessageDuplicate] = COALESCE(@@IsAS2MessageDuplicate, primaryImport.[IsAS2MessageDuplicate]),
                [DaysToCheckDuplicate] = COALESCE(@@DaysToCheckDuplicate, primaryImport.[DaysToCheckDuplicate]),
                [Filename] = COALESCE(@@Filename, primaryImport.[Filename]),
                [TrackingActivityID] = COALESCE(@@TrackingActivityID, primaryImport.[TrackingActivityID]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_AS2MessageActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_AS2MessageActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [ReceiverPartyName],
          [SenderPartyName],
          [AS2PartyRole],
          [AS2From],
          [AS2To],
          [MessageID],
          [MessageDateTime],
          [BTSInterchangeID],
          [BTSMessageID],
          [MdnProcessingStatus],
          [MessageEncryptionType],
          [IsMdnExpected],
          [MicAlgorithmType],
          [MessageSignatureType],
          [MessagePayloadContentKey],
          [MessageWireContentKey],
          [MessageMicValue],
          [TimeCreated],
          [RowFlags],
          [IsAS2MessageDuplicate],
          [DaysToCheckDuplicate],
          [Filename],
          [TrackingActivityID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@ReceiverPartyName,
                    @@SenderPartyName,
                    @@AS2PartyRole,
                    @@AS2From,
                    @@AS2To,
                    @@MessageID,
                    @@MessageDateTime,
                    @@BTSInterchangeID,
                    @@BTSMessageID,
                    @@MdnProcessingStatus,
                    @@MessageEncryptionType,
                    @@IsMdnExpected,
                    @@MicAlgorithmType,
                    @@MessageSignatureType,
                    @@MessagePayloadContentKey,
                    @@MessageWireContentKey,
                    @@MessageMicValue,
                    @@TimeCreated,
                    @@RowFlags,
                    @@IsAS2MessageDuplicate,
                    @@DaysToCheckDuplicate,
                    @@Filename,
                    @@TrackingActivityID,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  