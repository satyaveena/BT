CREATE PROCEDURE [dbo].[bam_BusinessMessageJournal_EnableContinuation]
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
              WHERE ActivityName = N'BusinessMessageJournal'

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
                  FROM [dbo].[bam_BusinessMessageJournal_Continuations] WITH (ROWLOCK, REPEATABLEREAD)
                  WHERE ActivityID = @@rootTrace
              END

              -- If current trace has continuations, set their parents to root trace
              UPDATE [dbo].[bam_BusinessMessageJournal_Continuations]
              SET ParentActivityID = @@rootTrace 
              FROM [dbo].[bam_BusinessMessageJournal_Continuations] WITH (INDEX(NCI_ParentActivityID))
              WHERE ParentActivityID = @ContinuationToken

              -- Check whether current instance we are about to merge into main trace is complete
              DECLARE @IsComplete BIT
              SELECT @IsComplete = IsComplete FROM [dbo].[bam_BusinessMessageJournal_Active]
              WHERE ActivityID = @ContinuationToken 

              -- Insert continuation marker if it's not completed
              IF (@IsComplete IS NULL) OR (@IsComplete = 0)
              BEGIN
                  INSERT [dbo].[bam_BusinessMessageJournal_Continuations] (ActivityID, ParentActivityID)
                  VALUES (@ContinuationToken, @@rootTrace)
              END

              -- Update the Relationships for the continuation
              UPDATE [dbo].[bam_BusinessMessageJournal_ActiveRelationships]
              SET ActivityID = @@rootTrace 
              WHERE ActivityID = @ContinuationToken

              -- Move the data from the continuation to the parent row 
              DECLARE @MessageTrackingID NVARCHAR (128)
              DECLARE @ActionType INT 
              DECLARE @ContainerActivityID NVARCHAR (128)
              DECLARE @ContainerType INT 
              DECLARE @BTSInterchangeID NVARCHAR (128)
              DECLARE @BTSMessageID NVARCHAR (128)
              DECLARE @BTSServiceInstanceID NVARCHAR (128)
              DECLARE @BTSHostName NVARCHAR (128)
              DECLARE @RoutedToPartyName NVARCHAR (256)
              DECLARE @LinkedMessageTrackingID NVARCHAR (128)
              DECLARE @TimeCreated DATETIME 
              
              SELECT 
                  @MessageTrackingID = [MessageTrackingID],
                  @ActionType = [ActionType],
                  @ContainerActivityID = [ContainerActivityID],
                  @ContainerType = [ContainerType],
                  @BTSInterchangeID = [BTSInterchangeID],
                  @BTSMessageID = [BTSMessageID],
                  @BTSServiceInstanceID = [BTSServiceInstanceID],
                  @BTSHostName = [BTSHostName],
                  @RoutedToPartyName = [RoutedToPartyName],
                  @LinkedMessageTrackingID = [LinkedMessageTrackingID],
                  @TimeCreated = [TimeCreated]
              FROM [dbo].[bam_BusinessMessageJournal_Active]
              WHERE ActivityID = @ContinuationToken
              
              EXEC [dbo].[bam_BusinessMessageJournal_UpsertInstance]
                  @@ActivityID = @@rootTrace,
                  
                  @@MessageTrackingID = @MessageTrackingID,
                  @@ActionType = @ActionType,
                  @@ContainerActivityID = @ContainerActivityID,
                  @@ContainerType = @ContainerType,
                  @@BTSInterchangeID = @BTSInterchangeID,
                  @@BTSMessageID = @BTSMessageID,
                  @@BTSServiceInstanceID = @BTSServiceInstanceID,
                  @@BTSHostName = @BTSHostName,
                  @@RoutedToPartyName = @RoutedToPartyName,
                  @@LinkedMessageTrackingID = @LinkedMessageTrackingID,
                  @@TimeCreated = @TimeCreated

              -- then delete the continuation row from the active instances table
              DELETE FROM [dbo].[bam_BusinessMessageJournal_Active] 
              WHERE ActivityID = @ContinuationToken
          
    END
  
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_BusinessMessageJournal_EnableContinuation] TO [bam_BusinessMessageJournal_EventWriter]
    AS [dbo];

