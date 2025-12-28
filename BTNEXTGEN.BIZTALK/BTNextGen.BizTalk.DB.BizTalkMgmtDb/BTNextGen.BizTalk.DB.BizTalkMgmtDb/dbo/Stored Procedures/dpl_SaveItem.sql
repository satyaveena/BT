CREATE PROCEDURE [dbo].[dpl_SaveItem]
(
 @ModuleId int,
 @Namespace nvarchar(256)  = NULL,
 @Name nvarchar(256),
 @Type nvarchar(50),
 @IsPipeline tinyint,
 @Guid uniqueidentifier,
 @SchemaType tinyint,
 @phantomAssemblyName nvarchar(512) = NULL
)

AS

DECLARE @AssemblyId int
SELECT @AssemblyId = @ModuleId

DECLARE @SystemApplicationId int
SELECT @SystemApplicationId = nID FROM bts_application WHERE isSystem = 1

IF( @SchemaType = 2 ) -- CLRTypeSchema
BEGIN
 SELECT @AssemblyId = nID
  FROM bts_assembly a
  WHERE a.nvcFullName = @phantomAssemblyName
 IF( @@ROWCOUNT = 0 )
 BEGIN
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
    @phantomAssemblyName, --@Name,
    N'1.0.0.0', --@Version,
    N'neutral', --@Culture,
    N'', --@PublicKeyToken,
    @phantomAssemblyName, --@FullName,
    1, --@VersionMajor,
    0, --@VersionMinor,
    0, --@VersionBuild,
    0, --@VersionRevision,
    GETUTCDATE(),
    SUSER_SNAME(),
    2, --@Type, 
    1, --@GroupId,
    N'', --@ModuleXml,
    1, -- @SystemAssembly
    @SystemApplicationId   -- BUGBUG: TODO: TBD: hard coded app ID
   )
  SELECT @AssemblyId = @@IDENTITY
 END
 INSERT INTO bts_libreference(idapp,idlib,refName)
  VALUES( @ModuleId, @AssemblyId, @phantomAssemblyName ) -- This is needed for proper removal of phantom items (CLR generated schemas)
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
      @ModuleId,
      @phantomAssemblyName,
   1, --@VersionMajor,
   0, --@VersionMinor,
   0, --@VersionBuild,
   0, --@VersionRevision,
      N'neutral',
      N'',
      N'DotNetSchema'
     )
END

DECLARE @ItemId int
SELECT @ItemId = id 
 FROM bts_item 
 WHERE AssemblyId = @AssemblyId AND
   Namespace = @Namespace AND
   Name = @Name AND
   SchemaType = 2 -- refcounting only for CLRSchemas
IF( @@ROWCOUNT = 0 )
BEGIN
 INSERT INTO bts_item ( 
   AssemblyId,
   Namespace,
   Name,
   Type,
   IsPipeline,
   Guid,
   SchemaType
  )
  VALUES (
   @AssemblyId,
   @Namespace,
   @Name,
   @Type,
   @IsPipeline,
   @Guid,
   @SchemaType
  )
 SELECT @ItemId = @@IDENTITY
END  
RETURN @ItemId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_SaveItem] TO [BTS_ADMIN_USERS]
    AS [dbo];

