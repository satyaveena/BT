CREATE PROCEDURE [dbo].[admsvr_GetPartyByAliasNameValue]
@nvcAliasName nvarchar(256),
@nvcAliasQualifier nvarchar(64),
@nvcAliasValue nvarchar(256),
@nvcSID nvarchar(256) OUTPUT,
@nvcName nvarchar(256) OUTPUT
AS
SELECT      @nvcSID = bts_party.nvcSID,
            @nvcName = bts_party.nvcName
FROM bts_party, bts_party_alias
WHERE       UPPER(bts_party_alias.nvcName) = UPPER(@nvcAliasName) AND
            UPPER(bts_party_alias.nvcQualifier) = UPPER(@nvcAliasQualifier) AND
            bts_party_alias.nvcValue LIKE @nvcAliasValue  AND
            bts_party_alias.nPartyID = bts_party.nID