CREATE  PROCEDURE [dbo].[admsvr_GetPartyBySID]
@nvcDomainUser nvarchar(256),
@nvcSID nvarchar(256) OUTPUT,
@nvcName nvarchar(256) OUTPUT
AS
SELECT 	@nvcSID = bts_party.nvcSID,
	@nvcName = bts_party.nvcName
FROM 	bts_party, bts_party_alias
WHERE 	UPPER(bts_party_alias.nvcQualifier) = 'WINDOWSUSER' AND
 	UPPER(bts_party_alias.nvcValue) = UPPER(@nvcDomainUser)  AND
	bts_party_alias.nPartyID = bts_party.nID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetPartyBySID] TO [BTS_HOST_USERS]
    AS [dbo];

