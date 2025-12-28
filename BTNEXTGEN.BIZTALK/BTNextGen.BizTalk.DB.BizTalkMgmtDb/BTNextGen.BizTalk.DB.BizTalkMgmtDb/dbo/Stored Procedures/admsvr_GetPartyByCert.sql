CREATE  PROCEDURE [dbo].[admsvr_GetPartyByCert]
@nvcSignatureHash nvarchar(256),
@nvcSID  nvarchar(256) OUTPUT,
@nvcName nvarchar(256) OUTPUT
AS
SELECT 	@nvcSID = bts_party.nvcSID,
	@nvcName = bts_party.nvcName
FROM 	bts_party
WHERE 	UPPER(bts_party.nvcSignatureCertHash) = UPPER(@nvcSignatureHash)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetPartyByCert] TO [BTS_HOST_USERS]
    AS [dbo];

