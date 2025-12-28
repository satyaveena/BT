CREATE PROCEDURE [dbo].[bam_ResendTrackingActivity_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@CorrelationId NVARCHAR (255) = NULL,
            
            @@AdapterPrefix NVARCHAR (255) = NULL,
            
            @@ResendCount INT  = NULL,
            
            @@MaxResendCount INT  = NULL,
            
            @@ResendInterval INT  = NULL,
            
            @@MaxRetryCount INT  = NULL,
            
            @@RetryInterval INT  = NULL,
            
            @@MessageContentId NVARCHAR (255) = NULL,
            
            @@ResendTimeout INT  = NULL,
            
            @@RetryTimeout INT  = NULL,
            
            @@BTSInterchangeID NVARCHAR (128) = NULL
            
        
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
                FROM [dbo].[bam_ResendTrackingActivity_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_ResendTrackingActivity_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_ResendTrackingActivity_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [CorrelationId],
          [AdapterPrefix],
          [ResendCount],
          [MaxResendCount],
          [ResendInterval],
          [MaxRetryCount],
          [RetryInterval],
          [MessageContentId],
          [ResendTimeout],
          [RetryTimeout],
          [BTSInterchangeID]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@CorrelationId, primaryImport.[CorrelationId]),
                    COALESCE(@@AdapterPrefix, primaryImport.[AdapterPrefix]),
                    COALESCE(@@ResendCount, primaryImport.[ResendCount]),
                    COALESCE(@@MaxResendCount, primaryImport.[MaxResendCount]),
                    COALESCE(@@ResendInterval, primaryImport.[ResendInterval]),
                    COALESCE(@@MaxRetryCount, primaryImport.[MaxRetryCount]),
                    COALESCE(@@RetryInterval, primaryImport.[RetryInterval]),
                    COALESCE(@@MessageContentId, primaryImport.[MessageContentId]),
                    COALESCE(@@ResendTimeout, primaryImport.[ResendTimeout]),
                    COALESCE(@@RetryTimeout, primaryImport.[RetryTimeout]),
                    COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_ResendTrackingActivity_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'ResendTrackingActivity'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_ResendTrackingActivity_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_ResendTrackingActivity_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_ResendTrackingActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_ResendTrackingActivity_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_ResendTrackingActivity_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [CorrelationId] = COALESCE(@@CorrelationId, primaryImport.[CorrelationId]),
                [AdapterPrefix] = COALESCE(@@AdapterPrefix, primaryImport.[AdapterPrefix]),
                [ResendCount] = COALESCE(@@ResendCount, primaryImport.[ResendCount]),
                [MaxResendCount] = COALESCE(@@MaxResendCount, primaryImport.[MaxResendCount]),
                [ResendInterval] = COALESCE(@@ResendInterval, primaryImport.[ResendInterval]),
                [MaxRetryCount] = COALESCE(@@MaxRetryCount, primaryImport.[MaxRetryCount]),
                [RetryInterval] = COALESCE(@@RetryInterval, primaryImport.[RetryInterval]),
                [MessageContentId] = COALESCE(@@MessageContentId, primaryImport.[MessageContentId]),
                [ResendTimeout] = COALESCE(@@ResendTimeout, primaryImport.[ResendTimeout]),
                [RetryTimeout] = COALESCE(@@RetryTimeout, primaryImport.[RetryTimeout]),
                [BTSInterchangeID] = COALESCE(@@BTSInterchangeID, primaryImport.[BTSInterchangeID])    
            FROM [dbo].[bam_ResendTrackingActivity_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_ResendTrackingActivity_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [CorrelationId],
          [AdapterPrefix],
          [ResendCount],
          [MaxResendCount],
          [ResendInterval],
          [MaxRetryCount],
          [RetryInterval],
          [MessageContentId],
          [ResendTimeout],
          [RetryTimeout],
          [BTSInterchangeID]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@CorrelationId,
                    @@AdapterPrefix,
                    @@ResendCount,
                    @@MaxResendCount,
                    @@ResendInterval,
                    @@MaxRetryCount,
                    @@RetryInterval,
                    @@MessageContentId,
                    @@ResendTimeout,
                    @@RetryTimeout,
                    @@BTSInterchangeID    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  