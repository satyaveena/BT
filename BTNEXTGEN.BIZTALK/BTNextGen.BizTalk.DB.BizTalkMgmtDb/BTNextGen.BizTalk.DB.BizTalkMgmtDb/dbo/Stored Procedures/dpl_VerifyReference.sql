CREATE PROCEDURE [dbo].[dpl_VerifyReference] 
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int, 
 @Culture nvarchar(25),
 @PublicKeyToken nvarchar(256),
 @Type int,
 @ArtifactName nvarchar(256),
 @ArtifactType nvarchar(256)

AS
DECLARE @Id int

SELECT @Id = (
 SELECT TOP 1 nID
 FROM bts_assembly
 WHERE (nvcName = @Name) AND 
  nType = 2 AND
  nVersionMajor = @VersionMajor AND
  nVersionMinor = @VersionMinor AND
  nVersionBuild = @VersionBuild AND
  nVersionRevision = @VersionRevision AND
  nvcPublicKeyToken = @PublicKeyToken
 ORDER BY nvcPublicKeyToken DESC, nVersionMajor DESC, nVersionMinor DESC, nVersionBuild DESC, nVersionRevision DESC
)
 
/*IF @@ROWCOUNT = 0*/
IF (@Id IS NULL)
 RETURN 2 /* Indicate nonexistent module */

RETURN 0 /* Success */
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_VerifyReference] TO [BTS_ADMIN_USERS]
    AS [dbo];

