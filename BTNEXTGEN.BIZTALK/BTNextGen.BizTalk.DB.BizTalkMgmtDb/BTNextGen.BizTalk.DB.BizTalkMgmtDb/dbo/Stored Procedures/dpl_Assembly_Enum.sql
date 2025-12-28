CREATE PROCEDURE [dbo].[dpl_Assembly_Enum]
(
 @GroupName as nvarchar(256) = null,
 @FilterName as nvarchar(256) = N'%' -- default: enum all modules
)

AS
set nocount on
if (@GroupName is NULL)
 begin
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
  FROM [bts_assembly]
  WHERE ([nvcName] LIKE @FilterName)
   AND ([nSystemAssembly] = 0) -- Do not enumerate system assemblies
  ORDER BY
   [nvcName],
   [nVersionMajor],
   [nVersionMinor],
   [nVersionBuild],
   [nVersionRevision]
 end
else
 begin
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
  FROM [bts_assembly]
  JOIN [adm_Group] ON ([bts_assembly].[nGroupId] =  [adm_Group].[Id])
  WHERE ([adm_Group].[Name] = @GroupName) 
   AND ([nvcName] LIKE @FilterName)
   AND ([nSystemAssembly] = 0) -- Do not enumerate system assemblies
  ORDER BY
   [nvcName],
   [nVersionMajor],
   [nVersionMinor],
   [nVersionBuild],
   [nVersionRevision]
 end
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_Enum] TO [BTS_OPERATORS]
    AS [dbo];

