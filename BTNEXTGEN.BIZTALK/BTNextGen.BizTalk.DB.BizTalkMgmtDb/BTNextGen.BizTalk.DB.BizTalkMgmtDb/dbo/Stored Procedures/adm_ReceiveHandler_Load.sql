CREATE PROCEDURE [dbo].[adm_ReceiveHandler_Load]
@AdapterName nvarchar(256),
@HostName nvarchar(80)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 select
  adm_ReceiveHandler.Id,
  adm_Adapter.Name,
  adm_Host.Name,
  adm_ReceiveHandler.CustomCfg,
  --case (select count(*) from adm_DefaultReceiveHandler where adm_DefaultReceiveHandler.ReceveiHandlerId = adm_ReceiveHandler.Id)
  -- when 0 then 0 else -1 end as DefaultHandler,
  adm_Host.Name,
  adm_ReceiveHandler.DateModified,
  N'{' + convert(nvarchar(50),adm_ReceiveHandler.uidCustomCfgID) + N'}',
  N'{' + convert(nvarchar(50),adm_ReceiveHandler.uidReceiveLocationSSOAppID) + N'}',
  adm_ReceiveHandler.nvcDescription
 from
  adm_ReceiveHandler,
  adm_Adapter,
  adm_Host
 where
  adm_Adapter.Name = @AdapterName AND
  adm_Host.Name = @HostName AND
  adm_ReceiveHandler.AdapterId = adm_Adapter.Id AND
  adm_ReceiveHandler.HostId = adm_Host.Id

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveHandler_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveHandler_Load] TO [BTS_OPERATORS]
    AS [dbo];

