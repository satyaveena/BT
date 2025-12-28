CREATE PROCEDURE [dbo].[bam_active_credit_cards_EnableContinuation]
      (
      
              @ParentActivityID NVARCHAR(128),
              @ContinuationToken NVARCHAR(128)
        
      )
    
    AS
    BEGIN
      
              -- Locking mechanism to pause the primary import when needed
              DECLARE @@ActivityName NVARCHAR(256)
              SELECT @@ActivityName = ActivityName 
              FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, HOLDLOCK)
              WHERE ActivityName = N'active_credit_cards'

              -- The goal is to find the parent activity of the activity we are trying to update
              -- this information is stored in the continuations table. Since we are making decisions
              -- based on this value, we need to prevent users from updating this value after we read, or
              -- if we find nothing, we need to prevent them from inserting a value. To prevent insertion,
              -- we take an applock on the ActivityID and our protocol requires that before a continuation is
              -- added an exclusive applock will be taken on our id. To prevent it from changing, we use
              -- repeatable read locks. The value could change if our current parent turns out to not be the 
              -- root and so when his continuation is established, we would be pointed to his new parent.
            
              DECLARE @@ret INT
              EXEC @@ret = sp_getapplock @ContinuationToken, 'Exclusive', 'Transaction', 100
              IF (@@ret < 0)  -- lock not granted
              BEGIN
                RAISERROR('Lock not granted', 16, 1)
                RETURN
              END
              
              -- Current trace's parent may also be continuation, find the root (main trace).
              DECLARE @@rootTrace NVARCHAR(128)

              DECLARE @@parentTemp NVARCHAR(128)
              SET @@parentTemp = @ParentActivityID
              WHILE (@@parentTemp IS NOT NULL)
              BEGIN
                  SET @@rootTrace = @@parentTemp

                  EXEC @@ret = sp_getapplock @@parentTemp, 'Shared', 'Transaction', 100
                  IF (@@ret < 0)  -- lock not granted
                  BEGIN
                    RAISERROR('Lock not granted', 16, 1)
                    RETURN
                  END

                  SET @@parentTemp = NULL
                  SELECT @@parentTemp = ParentActivityID 
                  FROM [dbo].[bam_active_credit_cards_Continuations] WITH (ROWLOCK, REPEATABLEREAD)
                  WHERE ActivityID = @@rootTrace
              END

              -- If current trace has continuations, set their parents to root trace
              UPDATE [dbo].[bam_active_credit_cards_Continuations]
              SET ParentActivityID = @@rootTrace 
              FROM [dbo].[bam_active_credit_cards_Continuations] WITH (INDEX(NCI_ParentActivityID))
              WHERE ParentActivityID = @ContinuationToken

              -- Check whether current instance we are about to merge into main trace is complete
              DECLARE @IsComplete BIT
              SELECT @IsComplete = IsComplete FROM [dbo].[bam_active_credit_cards_Active]
              WHERE ActivityID = @ContinuationToken 

              -- Insert continuation marker if it's not completed
              IF (@IsComplete IS NULL) OR (@IsComplete = 0)
              BEGIN
                  INSERT [dbo].[bam_active_credit_cards_Continuations] (ActivityID, ParentActivityID)
                  VALUES (@ContinuationToken, @@rootTrace)
              END

              -- Update the Relationships for the continuation
              UPDATE [dbo].[bam_active_credit_cards_ActiveRelationships]
              SET ActivityID = @@rootTrace 
              WHERE ActivityID = @ContinuationToken

              -- Move the data from the continuation to the parent row 
              DECLARE @CSCardRcv DATETIME 
              DECLARE @CardID NVARCHAR (50)
              DECLARE @ERPAccountNo NVARCHAR (50)
              DECLARE @Destination NVARCHAR (50)
              DECLARE @Alias NVARCHAR (150)
              DECLARE @ERPSent DATETIME 
              DECLARE @CSAckSent DATETIME 
              
              SELECT 
                  @CSCardRcv = [CSCardRcv],
                  @CardID = [CardID],
                  @ERPAccountNo = [ERPAccountNo],
                  @Destination = [Destination],
                  @Alias = [Alias],
                  @ERPSent = [ERPSent],
                  @CSAckSent = [CSAckSent]
              FROM [dbo].[bam_active_credit_cards_Active]
              WHERE ActivityID = @ContinuationToken
              
              EXEC [dbo].[bam_active_credit_cards_UpsertInstance]
                  @@ActivityID = @@rootTrace,
                  
                  @@CSCardRcv = @CSCardRcv,
                  @@CardID = @CardID,
                  @@ERPAccountNo = @ERPAccountNo,
                  @@Destination = @Destination,
                  @@Alias = @Alias,
                  @@ERPSent = @ERPSent,
                  @@CSAckSent = @CSAckSent

              -- then delete the continuation row from the active instances table
              DELETE FROM [dbo].[bam_active_credit_cards_Active] 
              WHERE ActivityID = @ContinuationToken
          
    END
  
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_active_credit_cards_EnableContinuation] TO [bam_active_credit_cards_EventWriter]
    AS [dbo];

