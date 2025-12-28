CREATE PROCEDURE [dbo].[edi_GetPartyNameForPartyId] (
	@PartyId int
)

AS

Select nvcName, nvcSID, nvcSignatureCertHash
from [dbo].[bts_party]
where nID = @PartyId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyNameForPartyId] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyNameForPartyId] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyNameForPartyId] TO [BTS_OPERATORS]
    AS [dbo];

