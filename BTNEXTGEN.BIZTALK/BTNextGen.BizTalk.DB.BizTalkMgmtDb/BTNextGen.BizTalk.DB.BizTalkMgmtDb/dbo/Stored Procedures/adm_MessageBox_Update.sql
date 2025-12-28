CREATE PROCEDURE [dbo].[adm_MessageBox_Update]
@DBServerName nvarchar(80),
@DBName nvarchar(128),
@DisableNewMsgPublication int,
@ConfigurationState int,
@Description nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction
 
  update adm_MessageBox
  set
   DateModified = GETUTCDATE(), 
   DisableNewMsgPublication = @DisableNewMsgPublication,
   ConfigurationState = @ConfigurationState,
   nvcDescription = @Description
  where
   DBServerName = @DBServerName AND
   DBName = @DBName

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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

