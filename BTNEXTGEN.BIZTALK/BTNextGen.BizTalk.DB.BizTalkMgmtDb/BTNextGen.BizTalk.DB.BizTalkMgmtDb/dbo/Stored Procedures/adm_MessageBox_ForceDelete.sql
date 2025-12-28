CREATE PROCEDURE [dbo].[adm_MessageBox_ForceDelete]
@DBServerName nvarchar(80),
@DBName nvarchar(128)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 -- Invoke internal method to remove msgbox record and related records, bypassing any constraint checking
 exec @ErrCode = adm_MessageBox_Internal_Delete @DBServerName, @DBName
 if ( @ErrCode <> 0 ) goto exit_proc

exit_proc:
 return @ErrCode
 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_MessageBox_ForceDelete] TO [BTS_ADMIN_USERS]
    AS [dbo];

