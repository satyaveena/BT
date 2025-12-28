CREATE VIEW [dbo].[bts_party]
WITH SCHEMABINDING 
AS
SELECT     PartnerId AS nID, Name AS nvcName, SID AS nvcSID, CertificateName AS nvcSignatureCert, CertificateHash AS nvcSignatureCertHash, DateModified, 
                      CustomData AS nvcCustomData
FROM         tpm.Partner

GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party] TO [BTS_OPERATORS]
    AS [dbo];

