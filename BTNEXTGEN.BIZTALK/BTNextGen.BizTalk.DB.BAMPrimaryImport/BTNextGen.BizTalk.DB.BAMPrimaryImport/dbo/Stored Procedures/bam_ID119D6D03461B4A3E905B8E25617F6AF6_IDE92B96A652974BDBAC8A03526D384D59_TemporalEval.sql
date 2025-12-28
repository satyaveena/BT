create procedure [dbo].[bam_ID119D6D03461B4A3E905B8E25617F6AF6_IDE92B96A652974BDBAC8A03526D384D59_TemporalEval]
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
  
    bamview.[TimeEnded],
  
    bamview.[TimeStarted] 
  
  

  
  

  from [dbo].[bam_CyberSourceFeedV_ViewCybersource Settlement Feed_View] as bamview       
  where
  (

  
    (( @ColumnName=N'TimeEnded')
      and
      (  ([bamview].[TimeEnded]< @Upperboundary) and
        ([bamview].[TimeEnded]>=  @Lowerboundary)
        ))

        
      or
    
  
    (( @ColumnName=N'TimeStarted')
      and
      (  ([bamview].[TimeStarted]< @Upperboundary) and
        ([bamview].[TimeStarted]>=  @Lowerboundary)
        ))

        
  
  )
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bam_ID119D6D03461B4A3E905B8E25617F6AF6_IDE92B96A652974BDBAC8A03526D384D59_TemporalEval] TO [BAM_ManagementNSReader]
    AS [dbo];

