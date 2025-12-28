CREATE PROCEDURE [dbo].[adm_Queue_Load]
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  host.Name
 from
  adm_Host host
 where
  host.Name = @HostName

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Queue_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_Queue_Load] TO [BTS_OPERATORS]
    AS [dbo];

