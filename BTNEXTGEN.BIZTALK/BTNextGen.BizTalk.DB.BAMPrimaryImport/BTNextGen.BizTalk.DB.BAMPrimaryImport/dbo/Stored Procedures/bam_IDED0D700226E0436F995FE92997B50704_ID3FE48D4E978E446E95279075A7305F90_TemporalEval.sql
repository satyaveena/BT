create procedure [dbo].[bam_IDED0D700226E0436F995FE92997B50704_ID3FE48D4E978E446E95279075A7305F90_TemporalEval]
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
  
    bamview.[EmailRecipient],
  
    bamview.[EmailSender],
  
    bamview.[EmailSent],
  
    bamview.[FileReceived] 
  
  

  
  

  from [dbo].[bam_EmailTotals_ViewEmailFromFile_View] as bamview       
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

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDED0D700226E0436F995FE92997B50704_ID3FE48D4E978E446E95279075A7305F90_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

