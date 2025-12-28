create procedure [dbo].[bam_ID42F0B2F7E9544D5F948849590FD0AE68_IDBBDB39A174094DCD80B0998BB187B13D_TemporalEval]
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
  
    bamview.[AccountNum],
  
    bamview.[CSAckUpdate],
  
    bamview.[ERPAckRcv],
  
    bamview.[NGRHeaderUpdate],
  
    bamview.[NGRLineUpdate],
  
    bamview.[PONum],
  
    bamview.[PORcvd],
  
    bamview.[POSentERP],
  
    bamview.[TargetERP],
  
    bamview.[TransNum] 
  
  

  
  

  from [dbo].[bam_ERPOrders_ViewERPOrders_View] as bamview       
  where
  (

  
    (( @ColumnName=N'CSAckUpdate')
      and
      (  ([bamview].[CSAckUpdate]< @Upperboundary) and
        ([bamview].[CSAckUpdate]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ERPAckRcv')
      and
      (  ([bamview].[ERPAckRcv]< @Upperboundary) and
        ([bamview].[ERPAckRcv]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'NGRHeaderUpdate')
      and
      (  ([bamview].[NGRHeaderUpdate]< @Upperboundary) and
        ([bamview].[NGRHeaderUpdate]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'NGRLineUpdate')
      and
      (  ([bamview].[NGRLineUpdate]< @Upperboundary) and
        ([bamview].[NGRLineUpdate]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'PORcvd')
      and
      (  ([bamview].[PORcvd]< @Upperboundary) and
        ([bamview].[PORcvd]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'POSentERP')
      and
      (  ([bamview].[POSentERP]< @Upperboundary) and
        ([bamview].[POSentERP]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID42F0B2F7E9544D5F948849590FD0AE68_IDBBDB39A174094DCD80B0998BB187B13D_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

