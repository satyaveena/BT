CREATE PROCEDURE [dbo].[edi_CreatePartyAlias] (
	@PartyId int,
	@AliasName nvarchar(256),
	@Qualifier nvarchar(64),
	@Value nvarchar(256)
)

AS

Declare @AliasId int
Select @AliasId = 0

Select @AliasId = nID from [dbo].[bts_party_alias] WHERE [nPartyID] = @PartyId and [nvcQualifier] = @Qualifier and [nvcValue] = @Value

if @AliasId = 0
BEGIN
	INSERT INTO [dbo].[bts_party_alias] (
		[nPartyID],
		[nvcName],
		[nvcQualifier],
		[nvcValue],
		[DateModified]
	) VALUES (
		@PartyId,
		@AliasName,
		@Qualifier,
		@Value,
		GetDate()
	)

	Select @AliasId = nID from [dbo].[bts_party_alias] WHERE [nPartyID] = @PartyId and [nvcQualifier] = @Qualifier and [nvcValue] = @Value
END
Select @AliasId as AliasId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreatePartyAlias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_CreatePartyAlias] TO [BTS_B2B_OPERATORS]
    AS [dbo];

