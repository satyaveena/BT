CREATE PROCEDURE [dbo].[MBOM_GetReceiveTransports]
AS
	SET NOCOUNT ON
	-- Special case: agent uses MSMQ ClassId for the service TypeId while InboundEngineCLSID is used for other transports
	declare @ServiceClassMSMQT as uniqueidentifier, @ServiceClassMessaging as uniqueidentifier
	select @ServiceClassMSMQT = N'{3D7A3F58-4BFB-4593-B99E-C2A5DC35A3B2}'
	
	select
			case
			when (adm_Adapter.MgmtCLSID = N'{9A7B0162-2CD5-4F61-B7EB-C40A3442A5F8}')
				then @ServiceClassMSMQT
				else adm_Adapter.InboundEngineCLSID
			end,
			Name
	from
		adm_Adapter
	where
		adm_Adapter.InboundEngineCLSID is not NULL -- skip protocols that don't support receive
	
	union all
	
	select
			uidGUID,
			nvcName
	from
		bts_receiveport
	RETURN

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MBOM_GetReceiveTransports] TO [BTS_ADMIN_USERS]
    AS [dbo];

