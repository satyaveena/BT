CREATE PROCEDURE [dbo].[dpl_FlipAssemblyHiddenStatus]
(
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int,
 @Culture nvarchar(25),
 @PublicKeyToken nvarchar(256),
 @Type int
)

AS
UPDATE bts_assembly
SET nType = -nType
WHERE
 (@Name = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision) and
 (@PublicKeyToken = nvcPublicKeyToken) and
 (@Type = ABS(nType))
    
IF (@@ROWCOUNT = 0)
 RETURN -1
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_FlipAssemblyHiddenStatus] TO [BTS_ADMIN_USERS]
    AS [dbo];

