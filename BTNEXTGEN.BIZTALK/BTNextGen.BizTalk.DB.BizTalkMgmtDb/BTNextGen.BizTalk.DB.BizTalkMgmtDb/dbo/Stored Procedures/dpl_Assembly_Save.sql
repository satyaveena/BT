CREATE PROCEDURE [dbo].[dpl_Assembly_Save]
(
 @ApplicationName nvarchar(256),
 @Guid nvarchar(256),
 @Name nvarchar(256),
 @Version nvarchar(256),
 @Culture nvarchar(256),
 @PublicKeyToken nvarchar(256),
 @FullName nvarchar(256),
 @VersionMajor int,
 @VersionMinor int,
 @VersionBuild int,
 @VersionRevision int,
 @Type int,
 @ModuleXml ntext = null
)

AS

DECLARE @GroupId int
SELECT @GroupId = ( SELECT TOP 1 [Id] -- Only one group, if nonexistent yet, so be it, NULL
FROM [adm_Group] )

DECLARE @SystemAssembly int
EXEC @SystemAssembly = dpl_IsSystemAssembly @Name = @FullName

DECLARE @ApplicationId int

IF( @SystemAssembly = 1 )
 BEGIN
 SELECT @ApplicationId = nID 
  FROM bts_application
  WHERE isSystem = 1
 END
ELSE
 BEGIN
  SELECT @ApplicationId = nID FROM bts_application WHERE [nvcName] = @ApplicationName
 END

 -- IF ( @@ROWCOUNT = 0 )  return error TODO: BUGBUG:

INSERT INTO bts_assembly
 (
  nvcName,
  nvcVersion,
  nvcCulture,
  nvcPublicKeyToken,
  nvcFullName,
  nVersionMajor,
  nVersionMinor,
  nVersionBuild,
  nVersionRevision,
  dtDateModified,
  nvcModifiedBy,
  nType,
  nGroupId,
  ntxtModuleXML,
  nSystemAssembly,
  nApplicationID
 )
 VALUES
 (
  @Name,
  @Version,
  @Culture,
  @PublicKeyToken,
  @FullName,
  @VersionMajor,
  @VersionMinor,
  @VersionBuild,
  @VersionRevision,
  GETUTCDATE(),
  SUSER_SNAME(),
  @Type,
  @GroupId,
  @ModuleXml,
  @SystemAssembly,
  @ApplicationId
 )
return @@IDENTITY -- AssemblyId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Assembly_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

