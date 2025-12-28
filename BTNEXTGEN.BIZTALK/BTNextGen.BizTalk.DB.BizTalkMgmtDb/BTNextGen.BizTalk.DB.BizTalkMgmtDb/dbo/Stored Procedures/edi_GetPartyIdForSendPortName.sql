CREATE PROCEDURE [dbo].[edi_GetPartyIdForSendPortName] (
	@SendPortName nvarchar(256)
)

AS

Select psp.nPartyID 
from [dbo].[bts_party_sendport] psp, [dbo].[bts_sendport] sp 
where psp.nSendPortID = sp.nID and sp.nvcName = @SendPortName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForSendPortName] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForSendPortName] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForSendPortName] TO [BTS_OPERATORS]
    AS [dbo];

