CREATE PROCEDURE [dbo].[dpl_UnlockGlobalPropertySchemas]
AS
UPDATE bts_assembly
SET nSystemAssembly = 0
WHERE
 (nvcName = N'Microsoft.BizTalk.GlobalPropertySchemas') and
 (nVersionMajor = 3) and
 (nVersionMinor = 0) and
 (nVersionBuild = 1) and
 (nVersionRevision = 0) and
 (nvcPublicKeyToken = N'31bf3856ad364e35')
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_UnlockGlobalPropertySchemas] TO [BTS_ADMIN_USERS]
    AS [dbo];

