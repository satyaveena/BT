CREATE PROCEDURE [dbo].[bam_TransactionSetActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@InterchangeControlNo NVARCHAR (14) = NULL,
            
            @@ReceiverID NVARCHAR (35) = NULL,
            
            @@SenderID NVARCHAR (35) = NULL,
            
            @@ReceiverQ NVARCHAR (4) = NULL,
            
            @@SenderQ NVARCHAR (4) = NULL,
            
            @@InterchangeDateTime DATETIME  = NULL,
            
            @@Direction INT  = NULL,
            
            @@ReceiverPartyName NVARCHAR (256) = NULL,
            
            @@SenderPartyName NVARCHAR (256) = NULL,
            
            @@ApplicationSender NVARCHAR (41) = NULL,
            
            @@ApplicationReceiver NVARCHAR (41) = NULL,
            
            @@GroupDateTime DATETIME  = NULL,
            
            @@GroupControlNo NVARCHAR (14) = NULL,
            
            @@TransactionSetId NVARCHAR (6) = NULL,
            
            @@DocType NVARCHAR (256) = NULL,
            
            @@TransactionSetControlNo NVARCHAR (14) = NULL,
            
            @@AckStatusCode INT  = NULL,
            
            @@BatchProcessing INT  = NULL,
            
            @@ProcessingDateTime DATETIME  = NULL,
            
            @@GroupOrdinal INT  = NULL,
            
            @@TransactionSetOrdinal INT  = NULL,
            
            @@MessageContentKey NVARCHAR (40) = NULL,
            
            @@TimeCreated DATETIME  = NULL,
            
            @@RowFlags INT  = NULL,
            
            @@TsCorrelationId NVARCHAR (32) = NULL,
            
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
                FROM [dbo].[bam_TransactionSetActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_TransactionSetActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_TransactionSetActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [InterchangeControlNo],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [ReceiverPartyName],
          [SenderPartyName],
          [ApplicationSender],
          [ApplicationReceiver],
          [GroupDateTime],
          [GroupControlNo],
          [TransactionSetId],
          [DocType],
          [TransactionSetControlNo],
          [AckStatusCode],
          [BatchProcessing],
          [ProcessingDateTime],
          [GroupOrdinal],
          [TransactionSetOrdinal],
          [MessageContentKey],
          [TimeCreated],
          [RowFlags],
          [TsCorrelationId],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                    COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                    COALESCE(@@SenderID, primaryImport.[SenderID]),
                    COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                    COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                    COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                    COALESCE(@@Direction, primaryImport.[Direction]),
                    COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                    COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                    COALESCE(@@ApplicationSender, primaryImport.[ApplicationSender]),
                    COALESCE(@@ApplicationReceiver, primaryImport.[ApplicationReceiver]),
                    COALESCE(@@GroupDateTime, primaryImport.[GroupDateTime]),
                    COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                    COALESCE(@@TransactionSetId, primaryImport.[TransactionSetId]),
                    COALESCE(@@DocType, primaryImport.[DocType]),
                    COALESCE(@@TransactionSetControlNo, primaryImport.[TransactionSetControlNo]),
                    COALESCE(@@AckStatusCode, primaryImport.[AckStatusCode]),
                    COALESCE(@@BatchProcessing, primaryImport.[BatchProcessing]),
                    COALESCE(@@ProcessingDateTime, primaryImport.[ProcessingDateTime]),
                    COALESCE(@@GroupOrdinal, primaryImport.[GroupOrdinal]),
                    COALESCE(@@TransactionSetOrdinal, primaryImport.[TransactionSetOrdinal]),
                    COALESCE(@@MessageContentKey, primaryImport.[MessageContentKey]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@TsCorrelationId, primaryImport.[TsCorrelationId]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_TransactionSetActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'TransactionSetActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_TransactionSetActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_TransactionSetActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_TransactionSetActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_TransactionSetActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_TransactionSetActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [InterchangeControlNo] = COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                [ReceiverID] = COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                [SenderID] = COALESCE(@@SenderID, primaryImport.[SenderID]),
                [ReceiverQ] = COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                [SenderQ] = COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                [InterchangeDateTime] = COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                [Direction] = COALESCE(@@Direction, primaryImport.[Direction]),
                [ReceiverPartyName] = COALESCE(@@ReceiverPartyName, primaryImport.[ReceiverPartyName]),
                [SenderPartyName] = COALESCE(@@SenderPartyName, primaryImport.[SenderPartyName]),
                [ApplicationSender] = COALESCE(@@ApplicationSender, primaryImport.[ApplicationSender]),
                [ApplicationReceiver] = COALESCE(@@ApplicationReceiver, primaryImport.[ApplicationReceiver]),
                [GroupDateTime] = COALESCE(@@GroupDateTime, primaryImport.[GroupDateTime]),
                [GroupControlNo] = COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                [TransactionSetId] = COALESCE(@@TransactionSetId, primaryImport.[TransactionSetId]),
                [DocType] = COALESCE(@@DocType, primaryImport.[DocType]),
                [TransactionSetControlNo] = COALESCE(@@TransactionSetControlNo, primaryImport.[TransactionSetControlNo]),
                [AckStatusCode] = COALESCE(@@AckStatusCode, primaryImport.[AckStatusCode]),
                [BatchProcessing] = COALESCE(@@BatchProcessing, primaryImport.[BatchProcessing]),
                [ProcessingDateTime] = COALESCE(@@ProcessingDateTime, primaryImport.[ProcessingDateTime]),
                [GroupOrdinal] = COALESCE(@@GroupOrdinal, primaryImport.[GroupOrdinal]),
                [TransactionSetOrdinal] = COALESCE(@@TransactionSetOrdinal, primaryImport.[TransactionSetOrdinal]),
                [MessageContentKey] = COALESCE(@@MessageContentKey, primaryImport.[MessageContentKey]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [TsCorrelationId] = COALESCE(@@TsCorrelationId, primaryImport.[TsCorrelationId]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_TransactionSetActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_TransactionSetActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [InterchangeControlNo],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [ReceiverPartyName],
          [SenderPartyName],
          [ApplicationSender],
          [ApplicationReceiver],
          [GroupDateTime],
          [GroupControlNo],
          [TransactionSetId],
          [DocType],
          [TransactionSetControlNo],
          [AckStatusCode],
          [BatchProcessing],
          [ProcessingDateTime],
          [GroupOrdinal],
          [TransactionSetOrdinal],
          [MessageContentKey],
          [TimeCreated],
          [RowFlags],
          [TsCorrelationId],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@InterchangeControlNo,
                    @@ReceiverID,
                    @@SenderID,
                    @@ReceiverQ,
                    @@SenderQ,
                    @@InterchangeDateTime,
                    @@Direction,
                    @@ReceiverPartyName,
                    @@SenderPartyName,
                    @@ApplicationSender,
                    @@ApplicationReceiver,
                    @@GroupDateTime,
                    @@GroupControlNo,
                    @@TransactionSetId,
                    @@DocType,
                    @@TransactionSetControlNo,
                    @@AckStatusCode,
                    @@BatchProcessing,
                    @@ProcessingDateTime,
                    @@GroupOrdinal,
                    @@TransactionSetOrdinal,
                    @@MessageContentKey,
                    @@TimeCreated,
                    @@RowFlags,
                    @@TsCorrelationId,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  