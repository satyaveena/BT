CREATE TABLE [dbo].[bam_EmailFromFile_Completed] (
    [RecordID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [ActivityID]         NVARCHAR (128)  NOT NULL,
    [FileReceived]       DATETIME        NULL,
    [FileContent3885]    NVARCHAR (3885) NULL,
    [EmailConstructed]   DATETIME        NULL,
    [EmailServer]        NVARCHAR (50)   NULL,
    [EmailRecipient]     NVARCHAR (256)  NULL,
    [EmailSender]        NVARCHAR (256)  NULL,
    [EmaiRecipientAdded] DATETIME        NULL,
    [EmailSent]          DATETIME        NULL,
    [LastModified]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCI_ActivityID]
    ON [dbo].[bam_EmailFromFile_Completed]([ActivityID] ASC);


GO
CREATE NONCLUSTERED INDEX [NCI_LastModified]
    ON [dbo].[bam_EmailFromFile_Completed]([LastModified] ASC);


GO
CREATE TRIGGER [dbo].[bam_EmailProcess_EmailProcess_RTACompletedTrigger] ON [dbo].[bam_EmailFromFile_Completed]
              FOR INSERT
              AS
              BEGIN
                
              IF @@ROWCOUNT = 0 RETURN -- No rows affected, exit immediately

              DECLARE @@par INT
              EXEC @@par = dbo.bam_Metadata_GetRTAMutex

              DECLARE @@timeSliceLength INT
              DECLARE @@timeWindow INT
              DECLARE @@currSliceRoundup DATETIME
              DECLARE @@numSlicesSoFar INT
              
              SELECT @@timeSliceLength = Timeslice, @@timeWindow = RTAWindow FROM [dbo].[bam_Metadata_RealTimeAggregations] WHERE (CubeName = N'EmailProcess' AND RtaName = N'EmailProcess')
              SET @@numSlicesSoFar = DATEDIFF(MINUTE, '2000-01-01', GETUTCDATE()) / @@timeSliceLength
              SET @@currSliceRoundup = DATEADD(MINUTE, @@numSlicesSoFar * @@timeSliceLength, '2000-01-01')

              -- Purge records older than (current time - RTA time window) from current partition
              DELETE FROM dbo.[bam_EmailProcess_EmailProcess_RTATable] WHERE Partition = @@par AND Timeslice < DATEADD(MINUTE, -@@timeWindow, @@currSliceRoundup)

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
              WHERE Partition = @@par AND Timeslice = @@currSliceRoundup

              IF @@ROWCOUNT=0
              INSERT dbo.[bam_EmailProcess_EmailProcess_RTATable]
              (
                  Partition,
                  Timeslice,
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
                  @@currSliceRoundup,
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
              
              -- Update the rta table change timestamp used by the SSAS RTA cube polling query
              UPDATE [dbo].[bam_Metadata_RealTimeAggregations]
              SET LastRtaDataUpdate = GETDATE()
              WHERE CubeName = 'EmailProcess' AND RtaName = 'EmailProcess'
          
              END
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailFromFile_Completed] TO [BAM_ManagementNSReader]
    AS [dbo];

