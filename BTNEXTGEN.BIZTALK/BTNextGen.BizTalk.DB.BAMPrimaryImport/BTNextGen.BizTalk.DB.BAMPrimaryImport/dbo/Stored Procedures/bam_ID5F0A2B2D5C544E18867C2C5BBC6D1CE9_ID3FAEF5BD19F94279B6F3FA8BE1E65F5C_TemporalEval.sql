create procedure [dbo].[bam_ID5F0A2B2D5C544E18867C2C5BBC6D1CE9_ID3FAEF5BD19F94279B6F3FA8BE1E65F5C_TemporalEval]
  (
  @TemporalID bigint,
  @ColumnName nvarchar(128),
  @Upperboundary datetime,
  @Lowerboundary datetime
  )
  as
  begin

  select @TemporalID as 'TemporalID',
  bamview.[ActivityID],
  
    bamview.[Alias],
  
    bamview.[CardID],
  
    bamview.[CSAckSent],
  
    bamview.[CSCardRcv],
  
    bamview.[Destination],
  
    bamview.[ERPAccountNo],
  
    bamview.[ERPSent] 
  
  

  
  

  from [dbo].[bam_active_CCards_view_Viewactive_credit_cards_View] as bamview       
  where
  (

  
    (( @ColumnName=N'CSAckSent')
      and
      (  ([bamview].[CSAckSent]< @Upperboundary) and
        ([bamview].[CSAckSent]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'CSCardRcv')
      and
      (  ([bamview].[CSCardRcv]< @Upperboundary) and
        ([bamview].[CSCardRcv]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ERPSent')
      and
      (  ([bamview].[ERPSent]< @Upperboundary) and
        ([bamview].[ERPSent]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID5F0A2B2D5C544E18867C2C5BBC6D1CE9_ID3FAEF5BD19F94279B6F3FA8BE1E65F5C_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

