CREATE VIEW [dbo].[bam_EmailProcess_EmailProcess_RTAWindowView]
        AS
          
            SELECT rtaData.*
            FROM dbo.[bam_EmailProcess_EmailProcess_RTATable] AS rtaData, [dbo].[bam_Metadata_RealTimeAggregations] AS rtaMetadata
            WHERE rtaMetadata.CubeName = N'EmailProcess' AND
                rtaMetadata.RtaName = N'EmailProcess'    AND
                (rtaData.Timeslice IS NULL OR 
                    (rtaData.Timeslice IS NOT NULL AND rtaData.Timeslice > DATEADD(MINUTE, -rtaMetadata.RTAWindow, DATEADD(MINUTE, (DATEDIFF(MINUTE, '2000-01-01', GETUTCDATE()) / rtaMetadata.Timeslice) * rtaMetadata.Timeslice, '2000-01-01')))) 
          