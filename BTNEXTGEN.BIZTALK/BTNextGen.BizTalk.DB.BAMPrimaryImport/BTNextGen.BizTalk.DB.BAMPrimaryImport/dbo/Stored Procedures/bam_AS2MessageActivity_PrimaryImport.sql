CREATE PROCEDURE [dbo].[bam_AS2MessageActivity_PrimaryImport]
      (
      
              @ActivityID NVARCHAR(128)=NULL,  
              @IsStartNew BIT,
              @IsComplete BIT,
              @ReceiverPartyName NVARCHAR (256) = NULL,
              @SenderPartyName NVARCHAR (256) = NULL,
              @AS2PartyRole INT  = NULL,
              @AS2From NVARCHAR (128) = NULL,
              @AS2To NVARCHAR (128) = NULL,
              @MessageID NVARCHAR (1000) = NULL,
              @MessageDateTime DATETIME  = NULL,
              @BTSInterchangeID NVARCHAR (128) = NULL,
              @BTSMessageID NVARCHAR (128) = NULL,
              @MdnProcessingStatus INT  = NULL,
              @MessageEncryptionType INT  = NULL,
              @IsMdnExpected INT  = NULL,
              @MicAlgorithmType INT  = NULL,
              @MessageSignatureType INT  = NULL,
              @MessagePayloadContentKey NVARCHAR (40) = NULL,
              @MessageWireContentKey NVARCHAR (40) = NULL,
              @MessageMicValue NVARCHAR (50) = NULL,
              @TimeCreated DATETIME  = NULL,
              @RowFlags INT  = NULL,
              @IsAS2MessageDuplicate INT  = NULL,
              @DaysToCheckDuplicate INT  = NULL,
              @Filename NVARCHAR (1000) = NULL,
              @TrackingActivityID NVARCHAR (128) = NULL,
              @AgreementName NVARCHAR (256) = NULL    
        
      )
    
    AS
    BEGIN
      
              -- Locking mechanism to pause the primary import when needed
              DECLARE @@ActivityName NVARCHAR(256)
              SELECT @@ActivityName = ActivityName 
              FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, HOLDLOCK) 
              WHERE ActivityName = N'AS2MessageActivity'

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
                  FROM [dbo].[bam_AS2MessageActivity_Continuations] WITH (ROWLOCK, REPEATABLEREAD)
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
                  FROM [dbo].[bam_AS2MessageActivity_Continuations] 
                  WHERE ActivityID = @ActivityID
              END
              
              -- UPSERT into the Primary Import Table
              DECLARE @utcNow DATETIME
              SET @utcNow = GETUTCDATE()

              EXEC [dbo].[bam_AS2MessageActivity_UpsertInstance]
              @@ActivityID = @RowID,
              @@IsVisible = @IsVisible,
              @@IsComplete = @MarkMainTraceComplete,
              @@LastModified = @utcNow,
              
              @@ReceiverPartyName = @ReceiverPartyName,
              @@SenderPartyName = @SenderPartyName,
              @@AS2PartyRole = @AS2PartyRole,
              @@AS2From = @AS2From,
              @@AS2To = @AS2To,
              @@MessageID = @MessageID,
              @@MessageDateTime = @MessageDateTime,
              @@BTSInterchangeID = @BTSInterchangeID,
              @@BTSMessageID = @BTSMessageID,
              @@MdnProcessingStatus = @MdnProcessingStatus,
              @@MessageEncryptionType = @MessageEncryptionType,
              @@IsMdnExpected = @IsMdnExpected,
              @@MicAlgorithmType = @MicAlgorithmType,
              @@MessageSignatureType = @MessageSignatureType,
              @@MessagePayloadContentKey = @MessagePayloadContentKey,
              @@MessageWireContentKey = @MessageWireContentKey,
              @@MessageMicValue = @MessageMicValue,
              @@TimeCreated = @TimeCreated,
              @@RowFlags = @RowFlags,
              @@IsAS2MessageDuplicate = @IsAS2MessageDuplicate,
              @@DaysToCheckDuplicate = @DaysToCheckDuplicate,
              @@Filename = @Filename,
              @@TrackingActivityID = @TrackingActivityID,
              @@AgreementName = @AgreementName
          
    END
  
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_AS2MessageActivity_PrimaryImport] TO [bam_AS2MessageActivity_EventWriter]
    AS [dbo];

