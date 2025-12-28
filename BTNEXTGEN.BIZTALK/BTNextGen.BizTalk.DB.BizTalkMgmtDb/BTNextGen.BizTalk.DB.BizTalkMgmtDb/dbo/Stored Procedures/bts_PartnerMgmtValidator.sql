CREATE PROCEDURE [dbo].[bts_PartnerMgmtValidator]
AS
	--// Party Signature Cert Validator
	declare @localized_string_error_certificate as NVARCHAR(4000)
	set @localized_string_error_certificate = N'Party Signature Certificate should be unique.'
	declare @nCount int
	select @nCount = count(nvcSignatureCertHash) from bts_party group by nvcSignatureCertHash having count(nvcSignatureCertHash) > 1
	if(@nCount > 1)
		RAISERROR(@localized_string_error_certificate , 16, 1)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_PartnerMgmtValidator] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_PartnerMgmtValidator] TO [BTS_OPERATORS]
    AS [dbo];

