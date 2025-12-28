create procedure [dbo].[bam_ID27C4966996AB4066829671CA825CC2C9_ID84E534ED179E44C3A750BA078576D11D_TemporalEval]
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
  
    bamview.[cardID],
  
    bamview.[email],
  
    bamview.[GetTokenReq],
  
    bamview.[MerchantRef],
  
    bamview.[ProfileReq],
  
    bamview.[ProfileResp],
  
    bamview.[ERPSent],
  
    bamview.[rcvCyber],
  
    bamview.[rcvERPReq],
  
    bamview.[Reason],
  
    bamview.[SendCyber],
  
    bamview.[SndTokenReq],
  
    bamview.[TargetERP] 
  
  

  
  

  from [dbo].[bam_PaymentERPTxn_ViewPaymentERPTxn_View] as bamview       
  where
  (

  
    (( @ColumnName=N'GetTokenReq')
      and
      (  ([bamview].[GetTokenReq]< @Upperboundary) and
        ([bamview].[GetTokenReq]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ProfileReq')
      and
      (  ([bamview].[ProfileReq]< @Upperboundary) and
        ([bamview].[ProfileReq]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ProfileResp')
      and
      (  ([bamview].[ProfileResp]< @Upperboundary) and
        ([bamview].[ProfileResp]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'ERPSent')
      and
      (  ([bamview].[ERPSent]< @Upperboundary) and
        ([bamview].[ERPSent]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'rcvCyber')
      and
      (  ([bamview].[rcvCyber]< @Upperboundary) and
        ([bamview].[rcvCyber]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'rcvERPReq')
      and
      (  ([bamview].[rcvERPReq]< @Upperboundary) and
        ([bamview].[rcvERPReq]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'SendCyber')
      and
      (  ([bamview].[SendCyber]< @Upperboundary) and
        ([bamview].[SendCyber]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'SndTokenReq')
      and
      (  ([bamview].[SndTokenReq]< @Upperboundary) and
        ([bamview].[SndTokenReq]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID27C4966996AB4066829671CA825CC2C9_ID84E534ED179E44C3A750BA078576D11D_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

