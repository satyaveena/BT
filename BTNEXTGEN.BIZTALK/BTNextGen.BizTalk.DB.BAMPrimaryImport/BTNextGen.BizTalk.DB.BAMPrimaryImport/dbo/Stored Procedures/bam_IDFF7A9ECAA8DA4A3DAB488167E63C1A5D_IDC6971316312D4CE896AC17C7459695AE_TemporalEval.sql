create procedure [dbo].[bam_IDFF7A9ECAA8DA4A3DAB488167E63C1A5D_IDC6971316312D4CE896AC17C7459695AE_TemporalEval]
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
  
    bamview.[DateCreated],
  
    bamview.[ExpirationMonth],
  
    bamview.[ExpirationYear],
  
    bamview.[ID_CreditCards],
  
    bamview.[ID_UserObjects],
  
    bamview.[Last4Digits],
  
    bamview.[EmailAddress] 
  
  

  
  

  from [dbo].[bam_ExpiredCreditCards_ViewExpiredCreditCards_View] as bamview       
  where
  (

  
    (( @ColumnName=N'DateCreated')
      and
      (  ([bamview].[DateCreated]< @Upperboundary) and
        ([bamview].[DateCreated]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_IDFF7A9ECAA8DA4A3DAB488167E63C1A5D_IDC6971316312D4CE896AC17C7459695AE_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

