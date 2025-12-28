CREATE PROCEDURE [dbo].[edi_GetPartyIdForPartyName] (
	@PartyName nvarchar(256)
)

AS

Select nID 
from [dbo].[bts_party]
where nvcName = @PartyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForPartyName] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForPartyName] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForPartyName] TO [BTS_OPERATORS]
    AS [dbo];

