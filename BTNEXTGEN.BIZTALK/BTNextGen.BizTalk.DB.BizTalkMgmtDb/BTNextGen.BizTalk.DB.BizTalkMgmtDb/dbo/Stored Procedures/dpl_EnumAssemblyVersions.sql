CREATE PROCEDURE [dbo].[dpl_EnumAssemblyVersions]
(
 @Name nvarchar(256)
)

AS

DECLARE @emptyguid uniqueidentifier
SELECT @emptyguid = CONVERT(uniqueidentifier,N'{00000000-0000-0000-0000-000000000000}')
SELECT
 nID AS id,
 @emptyguid as ModuleGuid,
 nvcName as ModuleName,
 nVersionMajor as VersionMajor,
 nVersionMinor as VersionMinor,
 nVersionBuild as VersionBuild,
 nVersionRevision as VersionRevision,
 dtDateModified as DateModified,
 nvcModifiedBy as ModifiedBy
FROM bts_assembly
WHERE nvcName = @Name
-- Important - we need the latest versions to go first, to support correct copying of postdeployment settings 
-- on deployment (like property tracking flag) to the subsequent versions
ORDER BY nVersionMajor DESC, nVersionMinor DESC, nVersionBuild DESC, nVersionRevision DESC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_EnumAssemblyVersions] TO [BTS_ADMIN_USERS]
    AS [dbo];

