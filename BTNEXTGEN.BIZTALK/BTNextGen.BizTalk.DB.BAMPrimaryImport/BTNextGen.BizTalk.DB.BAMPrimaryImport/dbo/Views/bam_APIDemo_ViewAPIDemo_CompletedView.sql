CREATE VIEW dbo.[bam_APIDemo_ViewAPIDemo_CompletedView]
        AS
          
          SELECT 
        RecordID,
        ActivityID,
        LastModified,
        -- Alias for single event and data item 
        [Data1],
        [Data2],
        [Data3],
        [Key]    
        -- Duration 
        -- Computed range dimension columns         
        -- Computed progress dimension columns 

          FROM dbo.[bam_APIDemo_ViewAPIDemo_CompletedAliasView]
      