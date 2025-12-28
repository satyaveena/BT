CREATE PROCEDURE [dbo].[dpl_Group_Enum]
(
 @FilterName as nvarchar(256) = N'%' -- default: enum all groups
)

AS

set nocount on
SELECT
 [Id] as [Id],
 [Name] as [Name]
FROM [adm_Group]
WHERE [Name] LIKE @FilterName
ORDER BY [Name]
set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Group_Enum] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Group_Enum] TO [BTS_OPERATORS]
    AS [dbo];

