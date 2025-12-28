CREATE PROCEDURE [dbo].[edi_GetPartyAliases](
	@PartyId int
)

AS

Select nvcQualifier, nvcValue
From [dbo].[bts_party_alias]
where nPartyID = @PartyId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliases] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliases] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliases] TO [BTS_OPERATORS]
    AS [dbo];

