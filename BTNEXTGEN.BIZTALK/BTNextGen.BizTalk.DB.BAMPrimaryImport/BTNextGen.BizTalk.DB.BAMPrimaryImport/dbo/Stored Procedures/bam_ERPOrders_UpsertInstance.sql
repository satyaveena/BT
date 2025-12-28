CREATE PROCEDURE [dbo].[bam_ERPOrders_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@PONum NVARCHAR (50) = NULL,
            
            @@TransNum NVARCHAR (50) = NULL,
            
            @@AccountNum NVARCHAR (50) = NULL,
            
            @@TargetERP NVARCHAR (20) = NULL,
            
            @@PORcvd DATETIME  = NULL,
            
            @@POSentERP DATETIME  = NULL,
            
            @@ERPAckRcv DATETIME  = NULL,
            
            @@NGRHeaderUpdate DATETIME  = NULL,
            
            @@NGRLineUpdate DATETIME  = NULL,
            
            @@CSAckUpdate DATETIME  = NULL
            
        
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
                FROM [dbo].[bam_ERPOrders_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_ERPOrders_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_ERPOrders_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [PONum],
          [TransNum],
          [AccountNum],
          [TargetERP],
          [PORcvd],
          [POSentERP],
          [ERPAckRcv],
          [NGRHeaderUpdate],
          [NGRLineUpdate],
          [CSAckUpdate]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@PONum, primaryImport.[PONum]),
                    COALESCE(@@TransNum, primaryImport.[TransNum]),
                    COALESCE(@@AccountNum, primaryImport.[AccountNum]),
                    COALESCE(@@TargetERP, primaryImport.[TargetERP]),
                    COALESCE(@@PORcvd, primaryImport.[PORcvd]),
                    COALESCE(@@POSentERP, primaryImport.[POSentERP]),
                    COALESCE(@@ERPAckRcv, primaryImport.[ERPAckRcv]),
                    COALESCE(@@NGRHeaderUpdate, primaryImport.[NGRHeaderUpdate]),
                    COALESCE(@@NGRLineUpdate, primaryImport.[NGRLineUpdate]),
                    COALESCE(@@CSAckUpdate, primaryImport.[CSAckUpdate])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_ERPOrders_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'ERPOrders'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_ERPOrders_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_ERPOrders_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_ERPOrders_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_ERPOrders_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_ERPOrders_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [PONum] = COALESCE(@@PONum, primaryImport.[PONum]),
                [TransNum] = COALESCE(@@TransNum, primaryImport.[TransNum]),
                [AccountNum] = COALESCE(@@AccountNum, primaryImport.[AccountNum]),
                [TargetERP] = COALESCE(@@TargetERP, primaryImport.[TargetERP]),
                [PORcvd] = COALESCE(@@PORcvd, primaryImport.[PORcvd]),
                [POSentERP] = COALESCE(@@POSentERP, primaryImport.[POSentERP]),
                [ERPAckRcv] = COALESCE(@@ERPAckRcv, primaryImport.[ERPAckRcv]),
                [NGRHeaderUpdate] = COALESCE(@@NGRHeaderUpdate, primaryImport.[NGRHeaderUpdate]),
                [NGRLineUpdate] = COALESCE(@@NGRLineUpdate, primaryImport.[NGRLineUpdate]),
                [CSAckUpdate] = COALESCE(@@CSAckUpdate, primaryImport.[CSAckUpdate])    
            FROM [dbo].[bam_ERPOrders_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_ERPOrders_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [PONum],
          [TransNum],
          [AccountNum],
          [TargetERP],
          [PORcvd],
          [POSentERP],
          [ERPAckRcv],
          [NGRHeaderUpdate],
          [NGRLineUpdate],
          [CSAckUpdate]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@PONum,
                    @@TransNum,
                    @@AccountNum,
                    @@TargetERP,
                    @@PORcvd,
                    @@POSentERP,
                    @@ERPAckRcv,
                    @@NGRHeaderUpdate,
                    @@NGRLineUpdate,
                    @@CSAckUpdate    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  