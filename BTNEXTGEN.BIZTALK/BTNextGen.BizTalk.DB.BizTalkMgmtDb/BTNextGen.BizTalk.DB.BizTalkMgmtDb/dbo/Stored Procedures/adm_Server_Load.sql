CREATE PROCEDURE [dbo].[adm_Server_Load]
@Name nvarchar(63)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 -- Update adm_Server record
 select
  adm_Server.Id, 
  adm_Server.Name, 
  adm_Server.DateModified
 from
  adm_Server
 where
  adm_Server.Name = @Name

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
 if ( @ErrCode <> 0 ) return @ErrCode

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Server_Load] TO [BTS_OPERATORS]
    AS [dbo];

