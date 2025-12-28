CREATE PROCEDURE [dbo].[dpl_EnumItems]
(
 @AssemblyId int,
 @ArtifactType nvarchar(256)
)

AS

SELECT id,FullName
FROM bts_item
WHERE
 AssemblyId = @AssemblyId and
 Type = @ArtifactType
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_EnumItems] TO [BTS_ADMIN_USERS]
    AS [dbo];

