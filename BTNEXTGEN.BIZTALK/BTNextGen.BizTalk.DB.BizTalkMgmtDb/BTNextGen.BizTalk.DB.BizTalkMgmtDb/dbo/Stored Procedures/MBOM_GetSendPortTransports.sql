CREATE PROCEDURE [dbo].[MBOM_GetSendPortTransports]
AS
	SET NOCOUNT ON
	
	select st.uidGUID, sp.nvcName, a.Name, st.nvcAddress, sp.bDynamic, sp.bTwoWay, st.bOrderedDelivery, st.bIsPrimary, sp.uidGUID
	from 
		bts_sendport as sp
		left outer join bts_sendport_transport as st on (sp.nID = st.nSendPortID)
		left outer join adm_Adapter as a on (st.nTransportTypeId = a.Id)
	
	RETURN

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MBOM_GetSendPortTransports] TO [BTS_ADMIN_USERS]
    AS [dbo];

