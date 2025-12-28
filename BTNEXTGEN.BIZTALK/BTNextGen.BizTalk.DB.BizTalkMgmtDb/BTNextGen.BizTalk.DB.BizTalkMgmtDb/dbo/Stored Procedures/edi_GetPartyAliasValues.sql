CREATE PROCEDURE [dbo].[edi_GetPartyAliasValues](
	@PartyName nvarchar(256),
	@Qualifier nvarchar(64)
)

AS

Select nvcValue
From [dbo].[bts_party_alias] alias, [dbo].[bts_party] party
where	party.nID = alias.nPartyID and
	party.nvcName = @PartyName and
	alias.nvcQualifier = @Qualifier
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliasValues] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliasValues] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyAliasValues] TO [BTS_OPERATORS]
    AS [dbo];

