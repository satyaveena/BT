CREATE VIEW dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_ActiveView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [TimeEnded],
        [TimeStarted]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_ActiveAliasView]
      