CREATE PROCEDURE [dbo].[bam_PaymentERPTxn_UpsertInstance]
      (
      
            @@ActivityID NVARCHAR(128) = NULL,  
            @@IsVisible BIT = NULL,
            @@IsComplete BIT = NULL,
            @@LastModified DATETIME = NULL,
            
            @@rcvERPReq DATETIME  = NULL,
            
            @@SndTokenReq DATETIME  = NULL,
            
            @@GetTokenReq DATETIME  = NULL,
            
            @@SendCyber DATETIME  = NULL,
            
            @@rcvCyber DATETIME  = NULL,
            
            @@ProfileReq DATETIME  = NULL,
            
            @@ProfileResp DATETIME  = NULL,
            
            @@ERPSent DATETIME  = NULL,
            
            @@MerchantRef NVARCHAR (100) = NULL,
            
            @@TargetERP NVARCHAR (50) = NULL,
            
            @@email NVARCHAR (100) = NULL,
            
            @@cardID NVARCHAR (100) = NULL,
            
            @@Reason NVARCHAR (50) = NULL
            
        
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
                FROM [dbo].[bam_PaymentERPTxn_Active] WITH (ROWLOCK, REPEATABLEREAD)
                WHERE ActivityID = @@ActivityID
            END
            
            -- Main trace is or has been marked as completed,
            -- check for continuation token
            IF (@@IsComplete = 1)
            BEGIN
                IF (EXISTS (SELECT continuations.ActivityID
                            FROM [dbo].[bam_PaymentERPTxn_Continuations] continuations WITH (ROWLOCK, REPEATABLEREAD)
                            WHERE @@ActivityID = continuations.ParentActivityID))
                    SET @@HasContinuations = 1
            END

            -- Main trace is complete and there're no more continuation tokens
            IF ((@@IsComplete = 1) AND (@@HasContinuations IS NULL))
            BEGIN
                -- Add new record to the completed instances table
                INSERT [dbo].[bam_PaymentERPTxn_Completed]
                (
                    ActivityID,
                    LastModified,
                    
          [rcvERPReq],
          [SndTokenReq],
          [GetTokenReq],
          [SendCyber],
          [rcvCyber],
          [ProfileReq],
          [ProfileResp],
          [ERPSent],
          [MerchantRef],
          [TargetERP],
          [email],
          [cardID],
          [Reason]
      
                )
                SELECT
                    @@ActivityID,
                    GETUTCDATE(),
                    
                    COALESCE(@@rcvERPReq, primaryImport.[rcvERPReq]),
                    COALESCE(@@SndTokenReq, primaryImport.[SndTokenReq]),
                    COALESCE(@@GetTokenReq, primaryImport.[GetTokenReq]),
                    COALESCE(@@SendCyber, primaryImport.[SendCyber]),
                    COALESCE(@@rcvCyber, primaryImport.[rcvCyber]),
                    COALESCE(@@ProfileReq, primaryImport.[ProfileReq]),
                    COALESCE(@@ProfileResp, primaryImport.[ProfileResp]),
                    COALESCE(@@ERPSent, primaryImport.[ERPSent]),
                    COALESCE(@@MerchantRef, primaryImport.[MerchantRef]),
                    COALESCE(@@TargetERP, primaryImport.[TargetERP]),
                    COALESCE(@@email, primaryImport.[email]),
                    COALESCE(@@cardID, primaryImport.[cardID]),
                    COALESCE(@@Reason, primaryImport.[Reason])
                FROM  [dbo].[bam_Metadata_Activities] activities
                LEFT JOIN [dbo].[bam_PaymentERPTxn_Active] primaryImport ON ISNULL(primaryImport.ActivityID, @@ActivityID) = @@ActivityID
                WHERE ((@@IsVisible = 1) OR (primaryImport.IsVisible = 1)) AND activities.ActivityName = N'PaymentERPTxn'

                -- If no record inserted, it's a hidden instance completed
                IF (@@ROWCOUNT = 0) 
                    GOTO HidenRowComplete

                -- Remove current activity from the active instances table
                DELETE FROM [dbo].[bam_PaymentERPTxn_Active] WHERE ActivityID = @@ActivityID

                -- Move relationship record to completed relationship table
                DECLARE @@recordID BIGINT
                SET @@recordID = @@identity

        
                -- Insert a new record to completed instances relationship table
                INSERT [dbo].[bam_PaymentERPTxn_CompletedRelationships](RecordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
                    SELECT @@recordID, ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData
                    FROM [dbo].[bam_PaymentERPTxn_ActiveRelationships] WHERE ActivityID = @@ActivityID
          

                -- Remove record from active instances relationship table
                DELETE FROM [dbo].[bam_PaymentERPTxn_ActiveRelationships] WHERE ActivityID = @@ActivityID

                RETURN
            END

        HidenRowComplete:

            -- Upsert into the Primary Import
        Again:
            UPDATE [dbo].[bam_PaymentERPTxn_Active]
            SET
                IsVisible = COALESCE(@@IsVisible, primaryImport.IsVisible),
                IsComplete = COALESCE(@@IsComplete, primaryImport.IsComplete),
                LastModified = GETUTCDATE(),
                
                [rcvERPReq] = COALESCE(@@rcvERPReq, primaryImport.[rcvERPReq]),
                [SndTokenReq] = COALESCE(@@SndTokenReq, primaryImport.[SndTokenReq]),
                [GetTokenReq] = COALESCE(@@GetTokenReq, primaryImport.[GetTokenReq]),
                [SendCyber] = COALESCE(@@SendCyber, primaryImport.[SendCyber]),
                [rcvCyber] = COALESCE(@@rcvCyber, primaryImport.[rcvCyber]),
                [ProfileReq] = COALESCE(@@ProfileReq, primaryImport.[ProfileReq]),
                [ProfileResp] = COALESCE(@@ProfileResp, primaryImport.[ProfileResp]),
                [ERPSent] = COALESCE(@@ERPSent, primaryImport.[ERPSent]),
                [MerchantRef] = COALESCE(@@MerchantRef, primaryImport.[MerchantRef]),
                [TargetERP] = COALESCE(@@TargetERP, primaryImport.[TargetERP]),
                [email] = COALESCE(@@email, primaryImport.[email]),
                [cardID] = COALESCE(@@cardID, primaryImport.[cardID]),
                [Reason] = COALESCE(@@Reason, primaryImport.[Reason])    
            FROM [dbo].[bam_PaymentERPTxn_Active] primaryImport
            WHERE primaryImport.ActivityID = @@ActivityID

            -- if the record was not found, then insert
            IF (@@ROWCOUNT = 0)
            BEGIN
                INSERT [dbo].[bam_PaymentERPTxn_Active]
                (
                    ActivityID,
                    IsVisible,
                    IsComplete,
                    LastModified,
                    
          [rcvERPReq],
          [SndTokenReq],
          [GetTokenReq],
          [SendCyber],
          [rcvCyber],
          [ProfileReq],
          [ProfileResp],
          [ERPSent],
          [MerchantRef],
          [TargetERP],
          [email],
          [cardID],
          [Reason]
      
                )
                SELECT
                    @@ActivityID,
                    @@IsVisible,
                    @@IsComplete,
                    GETUTCDATE(),
                    
                    @@rcvERPReq,
                    @@SndTokenReq,
                    @@GetTokenReq,
                    @@SendCyber,
                    @@rcvCyber,
                    @@ProfileReq,
                    @@ProfileResp,
                    @@ERPSent,
                    @@MerchantRef,
                    @@TargetERP,
                    @@email,
                    @@cardID,
                    @@Reason    

                -- check for primary key violation
                IF (@@error = 2627) 
                    GOTO Again
            END
        
    END
  