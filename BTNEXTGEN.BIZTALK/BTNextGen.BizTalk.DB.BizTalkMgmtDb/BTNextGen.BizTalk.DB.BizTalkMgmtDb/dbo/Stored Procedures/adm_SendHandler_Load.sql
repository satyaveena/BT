CREATE PROCEDURE [dbo].[adm_SendHandler_Load]
@AdapterName nvarchar(256)
AS
 set nocount on
 set xact_abort on

 declare @ErrCode as int
 set @ErrCode = 0

 declare @ServiceClassMSMQT as uniqueidentifier, @ServiceClassMessaging as uniqueidentifier
 
 select @ServiceClassMSMQT = UniqueId from adm_ServiceClass where Name = N'MSMQT'
 select @ServiceClassMessaging = UniqueId from adm_ServiceClass where Name = N'Messaging'

 select
  adm_SendHandler.Id,
  adm_Adapter.Name,
  adm_Host.Name,
  adm_SendHandler.CustomCfg,
  adm_SendHandler.SubscriptionId,
  N'{' + convert(nvarchar(50),adm_SendHandler.uidCustomCfgID) + N'}',
  N'{' + convert(nvarchar(50),adm_SendHandler.uidTransmitLocationSSOAppId) + N'}',
  adm_Adapter.OutboundEngineCLSID,
  case
   when (adm_Adapter.MgmtCLSID = N'{9A7B0162-2CD5-4F61-B7EB-C40A3442A5F8}')
    then @ServiceClassMSMQT
    else @ServiceClassMessaging
  end,
  adm_SendHandler.DateModified
 from
  adm_SendHandler,
  adm_Adapter,
  adm_Host
 where
  adm_Adapter.Name = @AdapterName AND
  adm_Host.Id = adm_SendHandler.HostId AND
  adm_Adapter.Id = adm_SendHandler.AdapterId AND
  adm_SendHandler.IsDefault = 1

 set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)

 set nocount off
 return @ErrCode
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Load] TO [BTS_OPERATORS]
    AS [dbo];

