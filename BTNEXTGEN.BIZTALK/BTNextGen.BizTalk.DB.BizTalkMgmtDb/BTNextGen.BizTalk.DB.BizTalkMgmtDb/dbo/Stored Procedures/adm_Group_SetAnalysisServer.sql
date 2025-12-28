CREATE PROCEDURE [dbo].[adm_Group_SetAnalysisServer]
@Name nvarchar(256),
@TrackAnalysisServerName nvarchar(80),
@TrackAnalysisDBName nvarchar(128)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction
  
  update adm_Group
  set
   TrackAnalysisServerName = @TrackAnalysisServerName, 
   TrackAnalysisDBName = @TrackAnalysisDBName,
   DateModified = GETUTCDATE()
  where
   Name = @Name

  set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
  if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 if(@ErrCode = 0)
  commit transaction
 else
 begin
  rollback transaction
  return @ErrCode
 end

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Group_SetAnalysisServer] TO [BTS_ADMIN_USERS]
    AS [dbo];

