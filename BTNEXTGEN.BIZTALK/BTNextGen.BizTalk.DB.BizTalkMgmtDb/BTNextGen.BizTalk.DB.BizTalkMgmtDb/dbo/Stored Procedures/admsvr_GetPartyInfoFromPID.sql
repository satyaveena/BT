CREATE  PROCEDURE [dbo].[admsvr_GetPartyInfoFromPID]
@nvcSID nvarchar(256) ,
@nvcParty nvarchar(256) OUTPUT,
@nvcQualifier nvarchar(256) OUTPUT
AS
SELECT 	
	@nvcQualifier 	= bts_party_alias.nvcQualifier,
	@nvcParty	= bts_party_alias.nvcValue 
FROM bts_party_alias
	INNER JOIN  bts_party ON bts_party_alias.nPartyID = bts_party.nID  
WHERE
 	bts_party.nvcSID = @nvcSID AND
 	bts_party.nvcName = bts_party_alias.nvcValue

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetPartyInfoFromPID] TO [BTS_HOST_USERS]
    AS [dbo];

