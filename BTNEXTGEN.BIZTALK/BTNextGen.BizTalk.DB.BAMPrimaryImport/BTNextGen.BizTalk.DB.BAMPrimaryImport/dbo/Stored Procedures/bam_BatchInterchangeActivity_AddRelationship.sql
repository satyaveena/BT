CREATE PROCEDURE [dbo].[bam_BatchInterchangeActivity_AddRelationship]
      (
      
              @ActivityID NVARCHAR(128),
              @ReferenceType NVARCHAR(128),
              @ReferenceName NVARCHAR(128),
          
              @ReferenceData NVARCHAR(1024),    
                
              @LongReferenceData NTEXT
        
      )
    
    AS
    BEGIN
      
              -- Locking mechanism to pause the primary import when needed
              DECLARE @@ActivityName NVARCHAR(256)
              SELECT @@ActivityName = ActivityName 
              FROM [dbo].[bam_Metadata_Activities] WITH (ROWLOCK, HOLDLOCK) 
              WHERE ActivityName = N'BatchInterchangeActivity'
              
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
              DECLARE @@RowID NVARCHAR(128)
              
              DECLARE @@ret INT
              DECLARE @@parentTemp NVARCHAR(128)
              SET @@parentTemp = @ActivityID
              WHILE (@@parentTemp IS NOT NULL)
              BEGIN
                  SET @@RowID = @@parentTemp

                  EXEC @@ret = sp_getapplock @@parentTemp, 'Shared', 'Transaction', 100
                  IF (@@ret < 0)  -- lock not granted
                  BEGIN
                    RAISERROR('Lock not granted', 16, 1)
                    RETURN
                  END

                  SET @@parentTemp = NULL
                  SELECT @@parentTemp = ParentActivityID 
                  FROM [dbo].[bam_BatchInterchangeActivity_Continuations] WITH (ROWLOCK, REPEATABLEREAD)
                  WHERE ActivityID = @@RowID
              END  

          
              DECLARE @RefData1 NVARCHAR(128)
              DECLARE @RefData2 NVARCHAR(896)

              IF (LEN(@ReferenceData) > 128)
              BEGIN
                SET @RefData1 = substring(@ReferenceData, 1, 128)
                SET @RefData2 = substring(@ReferenceData, 129, LEN(@ReferenceData) - 128)
              END
              ELSE
              BEGIN
                SET @RefData1 = @ReferenceData
                SET @RefData2 = ''
              END
              
              INSERT [dbo].[bam_BatchInterchangeActivity_ActiveRelationships](ActivityID, ReferenceType, ReferenceName, ReferenceData, ReferenceDataExtend, LongReferenceData)
              VALUES (@@RowID, @ReferenceType, @ReferenceName, @RefData1, @RefData2, @LongReferenceData)    
            
          
    END
  
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_BatchInterchangeActivity_AddRelationship] TO [bam_BatchInterchangeActivity_EventWriter]
    AS [dbo];

