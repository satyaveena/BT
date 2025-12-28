CREATE PROCEDURE [dbo].[edi_CreateAS2FromPartyAlias] (
	@PartyId int
)

AS

if not exists(Select * from [dbo].[bts_party_alias] WHERE [nPartyID] = @PartyId and [nvcQualifier] = 'AS2-From')
BEGIN
Declare @PartyName nvarchar(256)
Set @PartyName = (Select nvcName from [dbo].[bts_party] where nID = @PartyId)
	INSERT INTO [dbo].[bts_party_alias] (
		[nPartyID],
		[nvcName],
		[nvcQualifier],
		[nvcValue],
		[DateModified]
	) VALUES (
		@PartyId,
		'EDIINT-AS2',
		'AS2-From',
		@PartyName,
		GetDate()
	)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreateAS2FromPartyAlias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreateAS2FromPartyAlias] TO [BTS_B2B_OPERATORS]
    AS [dbo];

