create procedure [dbo].[bam_IDA3DE468142464EC8AAA6C2FCF5CDFD31_IDB876E293FF58400CB0C8D27714DA7828_TemporalEval]
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
  
    bamview.[EmailConstructed],
  
    bamview.[EmailRecipient],
  
    bamview.[EmailSender],
  
    bamview.[EmailSent],
  
    bamview.[EmailServer],
  
    bamview.[EmaiRecipientAdded],
  
    bamview.[FileContent3885],
  
    bamview.[FileReceived],
  
    bamview.[ProcessComplete],
  
  
    bamview.[FileToConstruct],
  
    bamview.[ProcessTime] 
  

  
  

  from [dbo].[bam_EmailTracking_ViewEmailFromFile_View] as bamview       
  where
  (

  
    (( @ColumnName=N'EmailConstructed')
      and
      (  ([bamview].[EmailConstructed]< @Upperboundary) and
        ([bamview].[EmailConstructed]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'EmailSent')
      and
      (  ([bamview].[EmailSent]< @Upperboundary) and
        ([bamview].[EmailSent]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'EmaiRecipientAdded')
      and
      (  ([bamview].[EmaiRecipientAdded]< @Upperboundary) and
        ([bamview].[EmaiRecipientAdded]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'FileReceived')
      and
      (  ([bamview].[FileReceived]< @Upperboundary) and
        ([bamview].[FileReceived]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ProcessComplete')
      and
      (  ([bamview].[ProcessComplete]< @Upperboundary) and
        ([bamview].[ProcessComplete]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDA3DE468142464EC8AAA6C2FCF5CDFD31_IDB876E293FF58400CB0C8D27714DA7828_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

