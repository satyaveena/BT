CREATE PROCEDURE [dbo].[bam_BatchingActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@BatchStatus INT  = NULL,
            
            @@DestinationPartyID NVARCHAR (40) = NULL,
            
            @@DestinationPartyName NVARCHAR (256) = NULL,
            
            @@ActivationTime DATETIME  = NULL,
            
            @@BatchOccurrenceCount INT  = NULL,
            
            @@EdiEncodingType INT  = NULL,
            
            @@BatchType INT  = NULL,
            
            @@TargetedBatchCount NVARCHAR (32) = NULL,
            
            @@ScheduledReleaseTime DATETIME  = NULL,
            
            @@BatchElementCount INT  = NULL,
            
            @@RejectedBatchElementCount INT  = NULL,
            
            @@BatchSize INT  = NULL,
            
            @@LastBatchAction INT  = NULL,
            
            @@CreationTime DATETIME  = NULL,
            
            @@ReleaseTime DATETIME  = NULL,
            
            @@BatchReleaseType INT  = NULL,
            
            @@BatchServiceID NVARCHAR (40) = NULL,
            
            @@ActivationMessageID NVARCHAR (40) = NULL,
            
            @@ReleaseMessageID NVARCHAR (40) = NULL,
            
            @@TimeCreated DATETIME  = NULL,
            
            @@RowFlags INT  = NULL,
            
            @@BatchCorrelationID NVARCHAR (40) = NULL,
            
            @@BatchName NVARCHAR (35) = NULL,
            
            @@BatchID NVARCHAR (35) = NULL,
            
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
                FROM [dbo].[bam_BatchingActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_BatchingActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_BatchingActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [BatchStatus],
          [DestinationPartyID],
          [DestinationPartyName],
          [ActivationTime],
          [BatchOccurrenceCount],
          [EdiEncodingType],
          [BatchType],
          [TargetedBatchCount],
          [ScheduledReleaseTime],
          [BatchElementCount],
          [RejectedBatchElementCount],
          [BatchSize],
          [LastBatchAction],
          [CreationTime],
          [ReleaseTime],
          [BatchReleaseType],
          [BatchServiceID],
          [ActivationMessageID],
          [ReleaseMessageID],
          [TimeCreated],
          [RowFlags],
          [BatchCorrelationID],
          [BatchName],
          [BatchID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@BatchStatus, primaryImport.[BatchStatus]),
                    COALESCE(@@DestinationPartyID, primaryImport.[DestinationPartyID]),
                    COALESCE(@@DestinationPartyName, primaryImport.[DestinationPartyName]),
                    COALESCE(@@ActivationTime, primaryImport.[ActivationTime]),
                    COALESCE(@@BatchOccurrenceCount, primaryImport.[BatchOccurrenceCount]),
                    COALESCE(@@EdiEncodingType, primaryImport.[EdiEncodingType]),
                    COALESCE(@@BatchType, primaryImport.[BatchType]),
                    COALESCE(@@TargetedBatchCount, primaryImport.[TargetedBatchCount]),
                    COALESCE(@@ScheduledReleaseTime, primaryImport.[ScheduledReleaseTime]),
                    COALESCE(@@BatchElementCount, primaryImport.[BatchElementCount]),
                    COALESCE(@@RejectedBatchElementCount, primaryImport.[RejectedBatchElementCount]),
                    COALESCE(@@BatchSize, primaryImport.[BatchSize]),
                    COALESCE(@@LastBatchAction, primaryImport.[LastBatchAction]),
                    COALESCE(@@CreationTime, primaryImport.[CreationTime]),
                    COALESCE(@@ReleaseTime, primaryImport.[ReleaseTime]),
                    COALESCE(@@BatchReleaseType, primaryImport.[BatchReleaseType]),
                    COALESCE(@@BatchServiceID, primaryImport.[BatchServiceID]),
                    COALESCE(@@ActivationMessageID, primaryImport.[ActivationMessageID]),
                    COALESCE(@@ReleaseMessageID, primaryImport.[ReleaseMessageID]),
                    COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                    COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                    COALESCE(@@BatchCorrelationID, primaryImport.[BatchCorrelationID]),
                    COALESCE(@@BatchName, primaryImport.[BatchName]),
                    COALESCE(@@BatchID, primaryImport.[BatchID]),
                    COALESCE(@@AgreementName, primaryImport.[AgreementName])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_BatchingActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'BatchingActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_BatchingActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_BatchingActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_BatchingActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_BatchingActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_BatchingActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [BatchStatus] = COALESCE(@@BatchStatus, primaryImport.[BatchStatus]),
                [DestinationPartyID] = COALESCE(@@DestinationPartyID, primaryImport.[DestinationPartyID]),
                [DestinationPartyName] = COALESCE(@@DestinationPartyName, primaryImport.[DestinationPartyName]),
                [ActivationTime] = COALESCE(@@ActivationTime, primaryImport.[ActivationTime]),
                [BatchOccurrenceCount] = COALESCE(@@BatchOccurrenceCount, primaryImport.[BatchOccurrenceCount]),
                [EdiEncodingType] = COALESCE(@@EdiEncodingType, primaryImport.[EdiEncodingType]),
                [BatchType] = COALESCE(@@BatchType, primaryImport.[BatchType]),
                [TargetedBatchCount] = COALESCE(@@TargetedBatchCount, primaryImport.[TargetedBatchCount]),
                [ScheduledReleaseTime] = COALESCE(@@ScheduledReleaseTime, primaryImport.[ScheduledReleaseTime]),
                [BatchElementCount] = COALESCE(@@BatchElementCount, primaryImport.[BatchElementCount]),
                [RejectedBatchElementCount] = COALESCE(@@RejectedBatchElementCount, primaryImport.[RejectedBatchElementCount]),
                [BatchSize] = COALESCE(@@BatchSize, primaryImport.[BatchSize]),
                [LastBatchAction] = COALESCE(@@LastBatchAction, primaryImport.[LastBatchAction]),
                [CreationTime] = COALESCE(@@CreationTime, primaryImport.[CreationTime]),
                [ReleaseTime] = COALESCE(@@ReleaseTime, primaryImport.[ReleaseTime]),
                [BatchReleaseType] = COALESCE(@@BatchReleaseType, primaryImport.[BatchReleaseType]),
                [BatchServiceID] = COALESCE(@@BatchServiceID, primaryImport.[BatchServiceID]),
                [ActivationMessageID] = COALESCE(@@ActivationMessageID, primaryImport.[ActivationMessageID]),
                [ReleaseMessageID] = COALESCE(@@ReleaseMessageID, primaryImport.[ReleaseMessageID]),
                [TimeCreated] = COALESCE(@@TimeCreated, primaryImport.[TimeCreated]),
                [RowFlags] = COALESCE(@@RowFlags, primaryImport.[RowFlags]),
                [BatchCorrelationID] = COALESCE(@@BatchCorrelationID, primaryImport.[BatchCorrelationID]),
                [BatchName] = COALESCE(@@BatchName, primaryImport.[BatchName]),
                [BatchID] = COALESCE(@@BatchID, primaryImport.[BatchID]),
                [AgreementName] = COALESCE(@@AgreementName, primaryImport.[AgreementName])    
            FROM [dbo].[bam_BatchingActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_BatchingActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [BatchStatus],
          [DestinationPartyID],
          [DestinationPartyName],
          [ActivationTime],
          [BatchOccurrenceCount],
          [EdiEncodingType],
          [BatchType],
          [TargetedBatchCount],
          [ScheduledReleaseTime],
          [BatchElementCount],
          [RejectedBatchElementCount],
          [BatchSize],
          [LastBatchAction],
          [CreationTime],
          [ReleaseTime],
          [BatchReleaseType],
          [BatchServiceID],
          [ActivationMessageID],
          [ReleaseMessageID],
          [TimeCreated],
          [RowFlags],
          [BatchCorrelationID],
          [BatchName],
          [BatchID],
          [AgreementName]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@BatchStatus,
                    @@DestinationPartyID,
                    @@DestinationPartyName,
                    @@ActivationTime,
                    @@BatchOccurrenceCount,
                    @@EdiEncodingType,
                    @@BatchType,
                    @@TargetedBatchCount,
                    @@ScheduledReleaseTime,
                    @@BatchElementCount,
                    @@RejectedBatchElementCount,
                    @@BatchSize,
                    @@LastBatchAction,
                    @@CreationTime,
                    @@ReleaseTime,
                    @@BatchReleaseType,
                    @@BatchServiceID,
                    @@ActivationMessageID,
                    @@ReleaseMessageID,
                    @@TimeCreated,
                    @@RowFlags,
                    @@BatchCorrelationID,
                    @@BatchName,
                    @@BatchID,
                    @@AgreementName    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  