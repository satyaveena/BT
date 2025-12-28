create procedure [dbo].[bam_IDA974DA43F29D4F57A2B9301C90EC744C_ID0AE84BE659874309B9447EAC16A67FFE_TemporalEval]
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
  
    bamview.[ProcessAttachment],
  
    bamview.[SendTolas] 
  
  

  
  

  from [dbo].[bam_FirstDataReconView_ViewFirstDataRecon_View] as bamview       
  where
  (

  
    (( @ColumnName=N'ProcessAttachment')
      and
      (  ([bamview].[ProcessAttachment]< @Upperboundary) and
        ([bamview].[ProcessAttachment]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'SendTolas')
      and
      (  ([bamview].[SendTolas]< @Upperboundary) and
        ([bamview].[SendTolas]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDA974DA43F29D4F57A2B9301C90EC744C_ID0AE84BE659874309B9447EAC16A67FFE_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

