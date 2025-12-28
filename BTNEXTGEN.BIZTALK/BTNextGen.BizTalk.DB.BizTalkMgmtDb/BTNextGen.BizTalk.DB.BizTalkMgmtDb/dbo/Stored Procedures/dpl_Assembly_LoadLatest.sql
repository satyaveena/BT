CREATE PROCEDURE [dbo].[dpl_Assembly_LoadLatest]
(
 @Name as nvarchar(256)
)

AS
set nocount on
set xact_abort on
SELECT TOP 1
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
FROM [bts_assembly]
WHERE ([nvcName]  = @Name)
ORDER BY
 [nvcName],
 [nVersionMajor] DESC,
 [nVersionMinor] DESC,
 [nVersionBuild] DESC,
 [nVersionRevision] DESC
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_LoadLatest] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_LoadLatest] TO [BTS_OPERATORS]
    AS [dbo];

