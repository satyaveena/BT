create procedure [dbo].[bam_ID479A949D030742C988E0992371EBC6FD_ID9F4DF99B98C64B5D9E604D221A620FDD_TemporalEval]
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
  
    bamview.[EmailSent],
  
    bamview.[FileReceived],
  
    bamview.[EmaiRecipientAdded],
  
  
    bamview.[ProcessTime],
  
    bamview.[EmailConstructed],
  

  
    bamview.[EmailProgress] 
  
  

  from [dbo].[bam_EmailProcess_ViewEmailFromFile_View] as bamview       
  where
  (

  
    (( @ColumnName=N'EmailSent')
      and
      (  ([bamview].[EmailSent]< @Upperboundary) and
        ([bamview].[EmailSent]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'FileReceived')
      and
      (  ([bamview].[FileReceived]< @Upperboundary) and
        ([bamview].[FileReceived]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'EmaiRecipientAdded')
      and
      (  ([bamview].[EmaiRecipientAdded]< @Upperboundary) and
        ([bamview].[EmaiRecipientAdded]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID479A949D030742C988E0992371EBC6FD_ID9F4DF99B98C64B5D9E604D221A620FDD_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

