CREATE PROCEDURE [dbo].[bam_AS2MdnActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@AS2PartyRole INT  = NULL,
            
            @@AS2From NVARCHAR (128) = NULL,
            
            @@AS2To NVARCHAR (128) = NULL,
            
            @@MessageID NVARCHAR (1000) = NULL,
            
            @@MdnDateTime DATETIME  = NULL,
            
            @@MdnDispositionType INT  = NULL,
            
            @@DispositionModifierExtType INT  = NULL,
            
            @@DispositionModifierExtDescription INT  = NULL,
            
            @@MdnEncryptionType INT  = NULL,
            
            @@MdnSignatureType INT  = NULL,
            
            @@MdnPayloadContentKey NVARCHAR (40) = NULL,
            
            @@MdnWireContentKey NVARCHAR (40) = NULL,
            
            @@MdnMicValue NVARCHAR (50) = NULL,
            
            @@TimeCreated DATETIME  = NULL,
            
            @@RowFlags INT  = NULL,
            
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
                FROM [dbo].[bam_AS2MdnActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_AS2MdnActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_AS2MdnActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [AS2PartyRole],
          [AS2From],
          [AS2To],
          [MessageID],
          [MdnDateTime],
          [MdnDispositionType],
          [DispositionModifierExtType],
          [DispositionModifierExtDescription],
          [MdnEncryptionType],
          [MdnSignatureType],
          [MdnPayloadContentKey],
          [MdnWireContentKey],
          [MdnMicValue],
          [TimeCreated],
          [RowFlags],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@AS2PartyRole, primaryImport.[AS2PartyRole]),
                    COALESCE(@@AS2From, primaryImport.[AS2From]),
                    COALESCE(@@AS2To, primaryImport.[AS2To]),
                    COALESCE(@@MessageID, primaryImport.[MessageID]),
                    COALESCE(@@MdnDateTime, primaryImport.[MdnDateTime]),
                    COALESCE(@@MdnDispositionType, primaryImport.[MdnDispositionType]),
                    COALESCE(@@DispositionModifierExtType, primaryImport.[DispositionModifierExtType]),
                    COALESCE(@@DispositionModifierExtDescription, primaryImport.[DispositionModifierExtDescription]),
                    COALESCE(@@MdnEncryptionType, primaryImport.[MdnEncryptionType]),
                    COALESCE(@@MdnSignatureType, primaryImport.[MdnSignatureType]),
                    COALESCE(@@MdnPayloadContentKey, primaryImport.[MdnPayloadContentKey]),
                    COALESCE(@@MdnWireContentKey, primaryImport.[MdnWireContentKey]),
                    COALESCE(@@MdnMicValue, primaryImport.[MdnMicValue]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_AS2MdnActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'AS2MdnActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_AS2MdnActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_AS2MdnActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_AS2MdnActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_AS2MdnActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_AS2MdnActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [AS2PartyRole] = COALESCE(@@AS2PartyRole, primaryImport.[AS2PartyRole]),
                [AS2From] = COALESCE(@@AS2From, primaryImport.[AS2From]),
                [AS2To] = COALESCE(@@AS2To, primaryImport.[AS2To]),
                [MessageID] = COALESCE(@@MessageID, primaryImport.[MessageID]),
                [MdnDateTime] = COALESCE(@@MdnDateTime, primaryImport.[MdnDateTime]),
                [MdnDispositionType] = COALESCE(@@MdnDispositionType, primaryImport.[MdnDispositionType]),
                [DispositionModifierExtType] = COALESCE(@@DispositionModifierExtType, primaryImport.[DispositionModifierExtType]),
                [DispositionModifierExtDescription] = COALESCE(@@DispositionModifierExtDescription, primaryImport.[DispositionModifierExtDescription]),
                [MdnEncryptionType] = COALESCE(@@MdnEncryptionType, primaryImport.[MdnEncryptionType]),
                [MdnSignatureType] = COALESCE(@@MdnSignatureType, primaryImport.[MdnSignatureType]),
                [MdnPayloadContentKey] = COALESCE(@@MdnPayloadContentKey, primaryImport.[MdnPayloadContentKey]),
                [MdnWireContentKey] = COALESCE(@@MdnWireContentKey, primaryImport.[MdnWireContentKey]),
                [MdnMicValue] = COALESCE(@@MdnMicValue, primaryImport.[MdnMicValue]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_AS2MdnActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_AS2MdnActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [AS2PartyRole],
          [AS2From],
          [AS2To],
          [MessageID],
          [MdnDateTime],
          [MdnDispositionType],
          [DispositionModifierExtType],
          [DispositionModifierExtDescription],
          [MdnEncryptionType],
          [MdnSignatureType],
          [MdnPayloadContentKey],
          [MdnWireContentKey],
          [MdnMicValue],
          [TimeCreated],
          [RowFlags],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@AS2PartyRole,
                    @@AS2From,
                    @@AS2To,
                    @@MessageID,
                    @@MdnDateTime,
                    @@MdnDispositionType,
                    @@DispositionModifierExtType,
                    @@DispositionModifierExtDescription,
                    @@MdnEncryptionType,
                    @@MdnSignatureType,
                    @@MdnPayloadContentKey,
                    @@MdnWireContentKey,
                    @@MdnMicValue,
                    @@TimeCreated,
                    @@RowFlags,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  