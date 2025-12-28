CREATE PROCEDURE [dbo].[dpl_GetAssemblyGroupDeployedTo]
(
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int
)

AS 
DECLARE @nId int
SELECT  @nId = nGroupId
FROM bts_assembly
WHERE
 (@Name = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision)

IF (@@ROWCOUNT > 0)
 BEGIN
  SELECT Name
  FROM adm_Group 
  WHERE Id = @nId
  IF (@@ROWCOUNT > 0)
   RETURN 1
 END
RETURN -1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyGroupDeployedTo] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyGroupDeployedTo] TO [BTS_OPERATORS]
    AS [dbo];

