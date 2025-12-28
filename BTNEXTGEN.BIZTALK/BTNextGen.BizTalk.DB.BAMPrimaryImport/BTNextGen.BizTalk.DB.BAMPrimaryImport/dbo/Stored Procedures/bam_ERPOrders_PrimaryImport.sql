CREATE PROCEDURE [dbo].[bam_ERPOrders_PrimaryImport]
      (
      
              @ActivityID NVARCHAR(128)=NULL,  
              @IsStartNew BIT,
              @IsComplete BIT,
              @PONum NVARCHAR (50) = NULL,
              @TransNum NVARCHAR (50) = NULL,
              @AccountNum NVARCHAR (50) = NULL,
              @TargetERP NVARCHAR (20) = NULL,
              @PORcvd DATETIME  = NULL,
              @POSentERP DATETIME  = NULL,
              @ERPAckRcv DATETIME  = NULL,
              @NGRHeaderUpdate DATETIME  = NULL,
              @NGRLineUpdate DATETIME  = NULL,
              @CSAckUpdate DATETIME  = NULL    
        
      )
    
    AS
    BEGIN
      
              -- Locking mechanism to pause the primary import when needed
              DECLARE @@ActivityName NVARCHAR(256)
              SELECT @@ActivityName = ActivityName 
              FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, HOLDLOCK) 
              WHERE ActivityName = N'ERPOrders'

              DECLARE @MarkMainTraceComplete BIT
              DECLARE @IsVisible BIT
                  
              -- The goal is to find the parent activity of the activity we are trying to update
              -- this information is stored in the continuations table. Since we are making decisions
              -- based on this value, we need to prevent users from updating this value after we read, or
              -- if we find nothing, we need to prevent them from inserting a value. To prevent insertion,
              -- we take an applock on the ActivityID and our protocol requires that before a continuation is
              -- added an exclusive applock will be taken on our id. To prevent it from changing, we use
              -- repeatable read locks. The value could change if our current parent turns out to not be the 
              -- root and so when his continuation is established, we would be pointed to his new parent.
            
              -- If current trace is a continuation, RowID is its parent activity ID
              -- otherwise RowID is current activity ID (main trace or a new trace)
              DECLARE @RowID NVARCHAR(128)

              DECLARE @@ret INT
              DECLARE @@parentTemp NVARCHAR(128)
              SET @@parentTemp = @ActivityID
              WHILE (@@parentTemp IS NOT NULL)
              BEGIN
                  SET @RowID = @@parentTemp

                  EXEC @@ret = sp_getapplock @@parentTemp, 'Shared', 'Transaction', 100
                  IF (@@ret < 0)  -- lock not granted
                  BEGIN
                    RAISERROR('Lock not granted', 16, 1)
                    RETURN
                  END

                  SET @@parentTemp = NULL
                  SELECT @@parentTemp = ParentActivityID 
                  FROM [dbo].[bam_ERPOrders_Continuations] WITH (ROWLOCK, REPEATABLEREAD)
                  WHERE ActivityID = @RowID
              END
              
              IF (@IsStartNew = 1)
              BEGIN
                  SET @IsVisible = 1
              END

              IF (@RowID = @ActivityID)    -- current trace is main trace or a new trace
              BEGIN
                  SET @MarkMainTraceComplete = @IsComplete
              END
              ELSE IF (@IsComplete = 1)    -- current trace is a continuation
              BEGIN
                  -- Try to delete it from continuation table if completed
                  DELETE 
                  FROM [dbo].[bam_ERPOrders_Continuations] 
                  WHERE ActivityID = @ActivityID
              END
              
              -- UPSERT into the Primary Import Table
              DECLARE @utcNow DATETIME
              SET @utcNow = GETUTCDATE()

              EXEC [dbo].[bam_ERPOrders_UpsertInstance]
              @@ActivityID = @RowID,
              @@IsVisible = @IsVisible,
              @@IsComplete = @MarkMainTraceComplete,
              @@LastModified = @utcNow,
              
              @@PONum = @PONum,
              @@TransNum = @TransNum,
              @@AccountNum = @AccountNum,
              @@TargetERP = @TargetERP,
              @@PORcvd = @PORcvd,
              @@POSentERP = @POSentERP,
              @@ERPAckRcv = @ERPAckRcv,
              @@NGRHeaderUpdate = @NGRHeaderUpdate,
              @@NGRLineUpdate = @NGRLineUpdate,
              @@CSAckUpdate = @CSAckUpdate
          
    END
  
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ERPOrders_PrimaryImport] TO [bam_ERPOrders_EventWriter]
    AS [dbo];

