CREATE PROCEDURE [dbo].[dpl_LoadItem]
(
 @ArtifactId int,
 @ModuleId int OUTPUT,
 @Namespace nvarchar(256) OUTPUT,
 @Name nvarchar(256) OUTPUT,
 @Type nvarchar(50) OUTPUT,
 @IsPipeline tinyint OUTPUT,
 @Guid uniqueidentifier OUTPUT,
 @SchemaType tinyint OUTPUT
)

AS

SELECT @ModuleId = AssemblyId,
  @Namespace = Namespace,
  @Name = Name,
  @Type = Type,
  @IsPipeline = IsPipeline,
  @Guid = Guid,
  @SchemaType = SchemaType
 
FROM bts_item 
WHERE id = @ArtifactId

IF ( @ModuleId IS NULL )
 RETURN -1

RETURN 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_LoadItem] TO [BTS_ADMIN_USERS]
    AS [dbo];

