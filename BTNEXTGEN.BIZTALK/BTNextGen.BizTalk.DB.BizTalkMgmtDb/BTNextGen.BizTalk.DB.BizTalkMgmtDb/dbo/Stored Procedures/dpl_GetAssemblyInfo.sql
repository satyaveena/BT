CREATE PROCEDURE [dbo].[dpl_GetAssemblyInfo]
(
 @Guid nvarchar(256),
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int, 
 @Culture nvarchar(256),
 @PublicKeyToken nvarchar(256),
 @Type int
)

AS 
DECLARE @nId int
SELECT  @nId = nID
FROM bts_assembly
WHERE
 (@Name = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision) and
 (@Culture = nvcCulture) and
 (@PublicKeyToken = nvcPublicKeyToken) and
 (@Type = nType)

IF (@@ROWCOUNT > 0)
 BEGIN
  SELECT nvcName, nType, nVersionMajor, nVersionMinor, nVersionBuild, nVersionRevision, nvcCulture, nvcPublicKeyToken
  FROM bts_assembly 
   INNER JOIN bts_libreference ON bts_assembly.nID = bts_libreference.idapp
  WHERE bts_libreference.idlib = @nId AND bts_libreference.idapp <> @nId
  RETURN 1
 END
ELSE
 RETURN 0
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyInfo] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyInfo] TO [BTS_OPERATORS]
    AS [dbo];

