CREATE VIEW [dbo].[bam_EmailProcess_EmailProcess_RTAView]
        AS
          
            SELECT
                -- Count measures
                SUM(_Count) _Count,
                
                SUM(_Count) [FileCount],
                
                -- Sum measures
                CAST(SUM([EmailConstructed]) AS FLOAT)/SUM(_Count) [ConstructTime],
                CAST(SUM([ProcessTime]) AS FLOAT)/SUM(_Count) [ProcessTime],    
                
                
                
                SUM([EmailConstructed]) [ConstructTime#SUM],
                
                SUM([ProcessTime]) [ProcessTime#SUM],
                
            -- Data dimensions
            -- Time dimensions
            -- Range dimensions
            -- Progress dimensions
    [EmailProgress],
                
                [EmailProgress1] = 
        CASE 
            WHEN [EmailProgress] = N'Construction' THEN N'NewFile' 
            WHEN [EmailProgress] = N'Sending' THEN N'NewFile' 
            WHEN [EmailProgress] = N'Sent' THEN N'NewFile' 
            ELSE NULL
        END,
     [EmailProgress2] = 
        CASE 
            WHEN [EmailProgress] = N'Construction' THEN N'Construction' 
            WHEN [EmailProgress] = N'Sending' THEN N'Constructed' 
            WHEN [EmailProgress] = N'Sent' THEN N'Constructed' 
            ELSE NULL
        END,
     [EmailProgress3] = 
        CASE 
            WHEN [EmailProgress] = N'Sending' THEN N'Sending' 
            WHEN [EmailProgress] = N'Sent' THEN N'Sent' 
            ELSE NULL
        END
            FROM dbo.[bam_EmailProcess_EmailProcess_RTATable] AS rtaData, [dbo].[bam_Metadata_RealTimeAggregations] AS rtaMetadata
            WHERE rtaMetadata.CubeName = N'EmailProcess' AND
                rtaMetadata.RtaName = N'EmailProcess'    AND
                (rtaData.Timeslice IS NULL OR 
                    (rtaData.Timeslice IS NOT NULL AND rtaData.Timeslice > DATEADD(MINUTE, -rtaMetadata.RTAWindow, DATEADD(MINUTE, (DATEDIFF(MINUTE, '2000-01-01', GETUTCDATE()) / rtaMetadata.Timeslice) * rtaMetadata.Timeslice, '2000-01-01')))) 
            GROUP BY 
            -- Data dimensions
            -- Time dimensions
            -- Range dimensions
            -- Progress dimensions
    [EmailProgress]
            HAVING (SUM(_Count) > 0)
          
GO
GRANT SELECT
    ON OBJECT::[dbo].[bam_EmailProcess_EmailProcess_RTAView] TO [bam_EmailProcess]
    AS [dbo];

