CREATE PROCEDURE [dbo].[dpl_Assembly_UsedBy_Enum]
(
 @ModuleId as int
)

AS

SELECT
 [nID] as [Id],
 [nvcName] as [Name],
 [nVersionMajor] as [VersionMajor],
 [nVersionMinor] as [VersionMinor],
 [nVersionBuild] as [VersionBuild],
 [nVersionRevision] as [VersionRevision],
 [nvcCulture] as [Culture],
 [nvcPublicKeyToken] as [PublicKeyToken],
 [dtDateModified] as [DateModified],
 [nvcModifiedBy] as [ModifiedBy]
FROM [bts_assembly] JOIN [bts_libreference]
   ON ([nID] = [idapp])
WHERE ([idlib] = @ModuleId)
ORDER BY
 [nvcName],
 [nVersionMajor],
 [nVersionMinor],
 [nVersionBuild],
 [nVersionRevision]

RETURN @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_UsedBy_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_UsedBy_Enum] TO [BTS_OPERATORS]
    AS [dbo];

