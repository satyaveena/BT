CREATE PROCEDURE [dbo].[dpl_Assembly_Load]
(
 @Name as nvarchar(256),
 @VersionMajor as int,
 @VersionMinor as int,
 @VersionBuild as int,
 @VersionRevision as int,
 @Culture AS nvarchar(256),
 @PublicKeyToken AS nvarchar(256)
)

AS
set nocount on
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
FROM [bts_assembly] WITH (NOLOCK)
WHERE
 ([nvcName]  = @Name) and
 ([nVersionMajor] = @VersionMajor) and
 ([nVersionMinor] = @VersionMinor) and
 ([nVersionBuild] = @VersionBuild) and
 ([nVersionRevision] = @VersionRevision) and
 ([nvcCulture] = @Culture) and
 ([nvcPublicKeyToken] = @PublicKeyToken)
ORDER BY
 [nvcName],
 [nvcPublicKeyToken],
 [nvcCulture],
 [nVersionMajor],
 [nVersionMinor],
 [nVersionBuild],
 [nVersionRevision]
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_Load] TO [BTS_OPERATORS]
    AS [dbo];

