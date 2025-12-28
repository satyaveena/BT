CREATE  PROCEDURE [dbo].[admsvr_GetGroupSigningCert]
@nvcGroupName nvarchar(256),
@nvcHostName nvarchar(256),
@nvcGroupSignCertName nvarchar(256) OUTPUT,
@nvcHostSignCertName  nvarchar(256) OUTPUT
AS
SELECT @nvcGroupSignCertName = SignCertThumbprint
FROM 	adm_Group
WHERE Name = @nvcGroupName
SELECT @nvcHostSignCertName = N''

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetGroupSigningCert] TO [BTS_HOST_USERS]
    AS [dbo];

