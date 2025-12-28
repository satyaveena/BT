CREATE PROCEDURE [dbo].[edi_GetPartyIdForAlias] (
	@Qualifier nvarchar(64),
	@Value nvarchar(256),
	@IgnoreCase bit
	
)

AS

if (@IgnoreCase = 1)
begin
	Select nPartyID from [dbo].[bts_party_alias] where nvcQualifier = @Qualifier and nvcValue = @Value
end
else
begin
	Select nPartyID from [dbo].[bts_party_alias] where nvcQualifier = @Qualifier 
	and (CAST(nvcValue AS VarBinary(256)) = CAST(@Value as VarBinary(256)))
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForAlias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForAlias] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPartyIdForAlias] TO [BTS_OPERATORS]
    AS [dbo];

