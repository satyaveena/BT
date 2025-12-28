CREATE PROCEDURE [dbo].[dpl_SaveReference]
(
 @ReferringModuleId int,
 @AssemblyName nvarchar(256),
 @VersionMajor nvarchar(12),
 @VersionMinor nvarchar(12),
 @VersionBuild nvarchar(12),
 @VersionRevision nvarchar(12),
 @Culture nvarchar(25),
 @PublicKeyToken nvarchar(256),
 @ArtifactName nvarchar(256)
)

AS
INSERT INTO bts_itemreference (
  nReferringAssemblyID,
  nvcAssemblyName,
  nvcVersionMajor,
  nvcVersionMinor,
  nvcVersionBuild,
  nvcVersionRevision,
  nvcCulture,
  nvcPublicKeyToken,
  nvcItemName
 ) 
 VALUES (
  @ReferringModuleId,
  @AssemblyName,
  @VersionMajor,
  @VersionMinor,
  @VersionBuild,
  @VersionRevision,
  @Culture,
  @PublicKeyToken,
  @ArtifactName
 )
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveReference] TO [BTS_ADMIN_USERS]
    AS [dbo];

