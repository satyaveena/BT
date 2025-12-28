CREATE PROCEDURE [dbo].[adm_Server_Update]
@Name nvarchar(63)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 begin transaction

  -- Update adm_Server record
  update adm_Server
  set
   DateModified = GETUTCDATE()
  where
   adm_Server.Name = @Name

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
    ON OBJECT::[dbo].[adm_Server_Update] TO [BTS_ADMIN_USERS]
    AS [dbo];

