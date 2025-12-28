CREATE PROCEDURE [dbo].[bam_FunctionalAckActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@InterchangeActivityID NVARCHAR (128) = NULL,
            
            @@GroupControlNo NVARCHAR (14) = NULL,
            
            @@InterchangeControlNo NVARCHAR (14) = NULL,
            
            @@ReceiverID NVARCHAR (35) = NULL,
            
            @@SenderID NVARCHAR (35) = NULL,
            
            @@ReceiverQ NVARCHAR (4) = NULL,
            
            @@SenderQ NVARCHAR (4) = NULL,
            
            @@InterchangeDateTime DATETIME  = NULL,
            
            @@Direction INT  = NULL,
            
            @@AckProcessingStatus INT  = NULL,
            
            @@AckStatusCode INT  = NULL,
            
            @@DeliveredTSCount INT  = NULL,
            
            @@AcceptedTSCount INT  = NULL,
            
            @@AckIcn NVARCHAR (14) = NULL,
            
            @@AckIcnDate NVARCHAR (6) = NULL,
            
            @@AckIcnTime NVARCHAR (4) = NULL,
            
            @@ErrorCode1 INT  = NULL,
            
            @@ErrorCode2 INT  = NULL,
            
            @@ErrorCode3 INT  = NULL,
            
            @@ErrorCode4 INT  = NULL,
            
            @@ErrorCode5 INT  = NULL,
            
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
                FROM [dbo].[bam_FunctionalAckActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_FunctionalAckActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_FunctionalAckActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [InterchangeActivityID],
          [GroupControlNo],
          [InterchangeControlNo],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [AckProcessingStatus],
          [AckStatusCode],
          [DeliveredTSCount],
          [AcceptedTSCount],
          [AckIcn],
          [AckIcnDate],
          [AckIcnTime],
          [ErrorCode1],
          [ErrorCode2],
          [ErrorCode3],
          [ErrorCode4],
          [ErrorCode5],
          [TimeCreated],
          [RowFlags],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@InterchangeActivityID, primaryImport.[InterchangeActivityID]),
                    COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                    COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                    COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                    COALESCE(@@SenderID, primaryImport.[SenderID]),
                    COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                    COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                    COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                    COALESCE(@@Direction, primaryImport.[Direction]),
                    COALESCE(@@AckProcessingStatus, primaryImport.[AckProcessingStatus]),
                    COALESCE(@@AckStatusCode, primaryImport.[AckStatusCode]),
                    COALESCE(@@DeliveredTSCount, primaryImport.[DeliveredTSCount]),
                    COALESCE(@@AcceptedTSCount, primaryImport.[AcceptedTSCount]),
                    COALESCE(@@AckIcn, primaryImport.[AckIcn]),
                    COALESCE(@@AckIcnDate, primaryImport.[AckIcnDate]),
                    COALESCE(@@AckIcnTime, primaryImport.[AckIcnTime]),
                    COALESCE(@@ErrorCode1, primaryImport.[ErrorCode1]),
                    COALESCE(@@ErrorCode2, primaryImport.[ErrorCode2]),
                    COALESCE(@@ErrorCode3, primaryImport.[ErrorCode3]),
                    COALESCE(@@ErrorCode4, primaryImport.[ErrorCode4]),
                    COALESCE(@@ErrorCode5, primaryImport.[ErrorCode5]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_FunctionalAckActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'FunctionalAckActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_FunctionalAckActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_FunctionalAckActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_FunctionalAckActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_FunctionalAckActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_FunctionalAckActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [InterchangeActivityID] = COALESCE(@@InterchangeActivityID, primaryImport.[InterchangeActivityID]),
                [GroupControlNo] = COALESCE(@@GroupControlNo, primaryImport.[GroupControlNo]),
                [InterchangeControlNo] = COALESCE(@@InterchangeControlNo, primaryImport.[InterchangeControlNo]),
                [ReceiverID] = COALESCE(@@ReceiverID, primaryImport.[ReceiverID]),
                [SenderID] = COALESCE(@@SenderID, primaryImport.[SenderID]),
                [ReceiverQ] = COALESCE(@@ReceiverQ, primaryImport.[ReceiverQ]),
                [SenderQ] = COALESCE(@@SenderQ, primaryImport.[SenderQ]),
                [InterchangeDateTime] = COALESCE(@@InterchangeDateTime, primaryImport.[InterchangeDateTime]),
                [Direction] = COALESCE(@@Direction, primaryImport.[Direction]),
                [AckProcessingStatus] = COALESCE(@@AckProcessingStatus, primaryImport.[AckProcessingStatus]),
                [AckStatusCode] = COALESCE(@@AckStatusCode, primaryImport.[AckStatusCode]),
                [DeliveredTSCount] = COALESCE(@@DeliveredTSCount, primaryImport.[DeliveredTSCount]),
                [AcceptedTSCount] = COALESCE(@@AcceptedTSCount, primaryImport.[AcceptedTSCount]),
                [AckIcn] = COALESCE(@@AckIcn, primaryImport.[AckIcn]),
                [AckIcnDate] = COALESCE(@@AckIcnDate, primaryImport.[AckIcnDate]),
                [AckIcnTime] = COALESCE(@@AckIcnTime, primaryImport.[AckIcnTime]),
                [ErrorCode1] = COALESCE(@@ErrorCode1, primaryImport.[ErrorCode1]),
                [ErrorCode2] = COALESCE(@@ErrorCode2, primaryImport.[ErrorCode2]),
                [ErrorCode3] = COALESCE(@@ErrorCode3, primaryImport.[ErrorCode3]),
                [ErrorCode4] = COALESCE(@@ErrorCode4, primaryImport.[ErrorCode4]),
                [ErrorCode5] = COALESCE(@@ErrorCode5, primaryImport.[ErrorCode5]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_FunctionalAckActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_FunctionalAckActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [InterchangeActivityID],
          [GroupControlNo],
          [InterchangeControlNo],
          [ReceiverID],
          [SenderID],
          [ReceiverQ],
          [SenderQ],
          [InterchangeDateTime],
          [Direction],
          [AckProcessingStatus],
          [AckStatusCode],
          [DeliveredTSCount],
          [AcceptedTSCount],
          [AckIcn],
          [AckIcnDate],
          [AckIcnTime],
          [ErrorCode1],
          [ErrorCode2],
          [ErrorCode3],
          [ErrorCode4],
          [ErrorCode5],
          [TimeCreated],
          [RowFlags],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@InterchangeActivityID,
                    @@GroupControlNo,
                    @@InterchangeControlNo,
                    @@ReceiverID,
                    @@SenderID,
                    @@ReceiverQ,
                    @@SenderQ,
                    @@InterchangeDateTime,
                    @@Direction,
                    @@AckProcessingStatus,
                    @@AckStatusCode,
                    @@DeliveredTSCount,
                    @@AcceptedTSCount,
                    @@AckIcn,
                    @@AckIcnDate,
                    @@AckIcnTime,
                    @@ErrorCode1,
                    @@ErrorCode2,
                    @@ErrorCode3,
                    @@ErrorCode4,
                    @@ErrorCode5,
                    @@TimeCreated,
                    @@RowFlags,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  