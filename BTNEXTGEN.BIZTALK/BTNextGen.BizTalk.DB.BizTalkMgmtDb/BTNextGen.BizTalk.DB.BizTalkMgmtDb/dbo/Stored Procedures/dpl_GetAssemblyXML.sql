CREATE PROCEDURE [dbo].[dpl_GetAssemblyXML]
(
 @Name nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int, 
 @VersionRevision int, 
 @Type int
)

AS 
SELECT ntxtModuleXML as ModuleXML
FROM bts_assembly
WHERE
 (@Name = nvcName) and
 (@VersionMajor = nVersionMajor) and
 (@VersionMinor = nVersionMinor) and
 (@VersionBuild = nVersionBuild) and
 (@VersionRevision = nVersionRevision) and
 (@Type = nType)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyXML] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_GetAssemblyXML] TO [BTS_OPERATORS]
    AS [dbo];

