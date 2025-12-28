CREATE PROCEDURE [dbo].[adm_ReceiveHandler_Enum]
AS
 set nocount on
 set xact_abort on

 select
  adm_ReceiveHandler.Id,
  adm_Adapter.Name,
  adm_Host.Name,
  adm_ReceiveHandler.CustomCfg,
  --case (select count(*) from adm_DefaultReceiveHandler where adm_DefaultReceiveHandler.ReceveiHandlerId = adm_ReceiveHandler.Id)
  -- when 0 then 0 else -1 end as DefaultHandler,
  --  adm_ReceiveHandler.VirtServerName,
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
  adm_Host.Id = adm_ReceiveHandler.HostId AND
  adm_Adapter.Id = adm_ReceiveHandler.AdapterId
 order by
  adm_Host.Name

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveHandler_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_ReceiveHandler_Enum] TO [BTS_OPERATORS]
    AS [dbo];

