CREATE PROCEDURE [dbo].[adm_SendHandler_Enum]
AS
 set nocount on
 set xact_abort on

 declare @ServiceClassMSMQT as uniqueidentifier, @ServiceClassMessaging as uniqueidentifier
 
 select @ServiceClassMSMQT = UniqueId from adm_ServiceClass where Name = N'MSMQT'
 select @ServiceClassMessaging = UniqueId from adm_ServiceClass where Name = N'Messaging'

 select
  adm_SendHandler.Id,
  adm_Adapter.Name,
  adm_Host.Name,
  adm_SendHandler.CustomCfg,
  adm_SendHandler.SubscriptionId,
  adm_SendHandler.uidCustomCfgID,
  adm_SendHandler.uidTransmitLocationSSOAppId,
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
  adm_Host.Id = adm_SendHandler.HostId AND
  adm_Adapter.Id = adm_SendHandler.AdapterId AND
  adm_SendHandler.IsDefault = 1
 order by
  adm_Host.Name

 set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adm_SendHandler_Enum] TO [BTS_OPERATORS]
    AS [dbo];

