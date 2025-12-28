CREATE TABLE [dbo].[bam_EmailFromFile_Active] (
    [ActivityID]         NVARCHAR (128)  NOT NULL,
    [IsVisible]          BIT             DEFAULT (NULL) NULL,
    [IsComplete]         BIT             DEFAULT (NULL) NULL,
    [FileReceived]       DATETIME        NULL,
    [FileContent3885]    NVARCHAR (3885) NULL,
    [EmailConstructed]   DATETIME        NULL,
    [EmailServer]        NVARCHAR (50)   NULL,
    [EmailRecipient]     NVARCHAR (256)  NULL,
    [EmailSender]        NVARCHAR (256)  NULL,
    [EmaiRecipientAdded] DATETIME        NULL,
    [EmailSent]          DATETIME        NULL,
    [LastModified]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([ActivityID] ASC)
);


GO
CREATE TRIGGER [dbo].[bam_EmailProcess_EmailProcess_RTAActiveTrigger] ON [dbo].[bam_EmailFromFile_Active]
              FOR INSERT, UPDATE, DELETE
              AS
              BEGIN
                
              IF @@ROWCOUNT = 0 RETURN -- No rows affected, exit immediately

              DECLARE @@par INT
              EXEC @@par = dbo.bam_Metadata_GetRTAMutex

              -- Remove deleted instances' contribution from the real-time aggregation
              IF EXISTS (SELECT ActivityID FROM deleted WHERE IsVisible = 1)
              BEGIN
                  UPDATE dbo.[bam_EmailProcess_EmailProcess_RTATable]
                  SET    
                      
            [EmailConstructed] = COALESCE([RtaTable].[EmailConstructed] -((CAST([deleted].[EmaiRecipientAdded] AS FLOAT) - CAST( [deleted].[FileReceived] AS FLOAT))*86400)
, [RtaTable].[EmailConstructed]),
                      
            [ProcessTime] = COALESCE([RtaTable].[ProcessTime] -((CAST([deleted].[EmailSent] AS FLOAT) - CAST( [deleted].[FileReceived] AS FLOAT))*86400)
, [RtaTable].[ProcessTime]),
                      _Count = [RtaTable]._Count - 1
                  FROM dbo.[bam_EmailProcess_EmailProcess_RTATable] RtaTable
                  JOIN deleted ON    
            -- Join data dimension
            -- Join time dimension
            -- Join range dimension 
            -- Join Progress dimension
    ([RtaTable].[EmailProgress] IS NULL AND COALESCE([deleted].[FileReceived], [deleted].[EmaiRecipientAdded], [deleted].[EmailSent]) IS NULL
        OR [RtaTable].[EmailProgress] = CASE
            WHEN [deleted].[EmailSent] IS NOT NULL THEN N'Sent'
            WHEN [deleted].[EmaiRecipientAdded] IS NOT NULL THEN N'Sending'
            WHEN [deleted].[FileReceived] IS NOT NULL THEN N'Construction'
            ELSE NULL
            END)
                  WHERE Partition = @@par AND Timeslice IS NULL

                  -- Insert negative contribution row
                  IF @@ROWCOUNT = 0
                  INSERT dbo.[bam_EmailProcess_EmailProcess_RTATable]
                  (
                      Partition, 
                      -- Measures
                      [EmailConstructed],
                      [ProcessTime],    
                      _Count,
                      
            -- Data dimensions
            -- Time dimensions
            -- Range dimensions
            -- Progress dimensions
    [EmailProgress]
                  )
                  SELECT 
                      @@par, 
                      -- Measures
                      - COALESCE(((CAST([deleted].[EmaiRecipientAdded] AS FLOAT) - CAST( [deleted].[FileReceived] AS FLOAT))*86400)
, 0),
                      - COALESCE(((CAST([deleted].[EmailSent] AS FLOAT) - CAST( [deleted].[FileReceived] AS FLOAT))*86400)
, 0),
                      -1,
                      
            -- Data dimensions
            -- Time dimensions
            -- Range dimensions
            -- Progress dimensions
                CASE
            WHEN [deleted].[EmailSent] IS NOT NULL THEN N'Sent'
            WHEN [deleted].[EmaiRecipientAdded] IS NOT NULL THEN N'Sending'
            WHEN [deleted].[FileReceived] IS NOT NULL THEN N'Construction'
                    ELSE NULL
                END
                  FROM deleted
                          
                  -- Clean up buckets which have zero instance count
                  DELETE FROM dbo.[bam_EmailProcess_EmailProcess_RTATable]
                  WHERE Partition = @@par AND Timeslice IS NULL AND 
                  
                  ([EmailConstructed] IS NULL OR [EmailConstructed] = 0) AND 
                  
                  ([ProcessTime] IS NULL OR [ProcessTime] = 0) AND 
                   _Count = 0
              END
              
              -- Add new instances' contribution to the real-time aggregation
              IF EXISTS (SELECT ActivityID FROM inserted WHERE IsVisible = 1)
              BEGIN
                  -- Try adding contribution to same partition and timeslice record
                  UPDATE dbo.[bam_EmailProcess_EmailProcess_RTATable]
                  SET    
                      
            [EmailConstructed] = COALESCE([RtaTable].[EmailConstructed] +((CAST([inserted].[EmaiRecipientAdded] AS FLOAT) - CAST( [inserted].[FileReceived] AS FLOAT))*86400)
, [RtaTable].[EmailConstructed]),
                      
            [ProcessTime] = COALESCE([RtaTable].[ProcessTime] +((CAST([inserted].[EmailSent] AS FLOAT) - CAST( [inserted].[FileReceived] AS FLOAT))*86400)
, [RtaTable].[ProcessTime]),
                      _Count = [RtaTable]._Count + 1
                  FROM dbo.[bam_EmailProcess_EmailProcess_RTATable] RtaTable
                  JOIN inserted ON 
            -- Join data dimension
            -- Join time dimension
            -- Join range dimension 
            -- Join Progress dimension
    ([RtaTable].[EmailProgress] IS NULL AND COALESCE([inserted].[FileReceived], [inserted].[EmaiRecipientAdded], [inserted].[EmailSent]) IS NULL
        OR [RtaTable].[EmailProgress] = CASE
            WHEN [inserted].[EmailSent] IS NOT NULL THEN N'Sent'
            WHEN [inserted].[EmaiRecipientAdded] IS NOT NULL THEN N'Sending'
            WHEN [inserted].[FileReceived] IS NOT NULL THEN N'Construction'
            ELSE NULL
            END)
                  WHERE Partition = @@par AND Timeslice IS NULL

                  -- If same partition and timeslice record doesn't exist before, create its own record
                  IF @@ROWCOUNT = 0
                  INSERT dbo.[bam_EmailProcess_EmailProcess_RTATable]
                  (
                      Partition, 
                      -- Measures
                      [EmailConstructed],
                      [ProcessTime],    
                      _Count,
                      
            -- Data dimensions
            -- Time dimensions
            -- Range dimensions
            -- Progress dimensions
    [EmailProgress]
                  )
                  SELECT 
                      @@par, 
                      -- Measures
                      COALESCE(((CAST([inserted].[EmaiRecipientAdded] AS FLOAT) - CAST( [inserted].[FileReceived] AS FLOAT))*86400)
, 0),
                      COALESCE(((CAST([inserted].[EmailSent] AS FLOAT) - CAST( [inserted].[FileReceived] AS FLOAT))*86400)
, 0),
                      1,
                      
                -- Data dimensions
                -- Time dimensions
                -- Range dimensions
                -- Progress dimensions
                CASE
            WHEN [inserted].[EmailSent] IS NOT NULL THEN N'Sent'
            WHEN [inserted].[EmaiRecipientAdded] IS NOT NULL THEN N'Sending'
            WHEN [inserted].[FileReceived] IS NOT NULL THEN N'Construction'
                    ELSE NULL
                END
                  FROM inserted
              END
              
              -- Update the rta table change timestamp used by the SSAS RTA cube polling query
              UPDATE [dbo].[bam_Metadata_RealTimeAggregations]
              SET LastRtaDataUpdate = GETDATE()
              WHERE CubeName = 'EmailProcess' AND RtaName = 'EmailProcess'
          
              END