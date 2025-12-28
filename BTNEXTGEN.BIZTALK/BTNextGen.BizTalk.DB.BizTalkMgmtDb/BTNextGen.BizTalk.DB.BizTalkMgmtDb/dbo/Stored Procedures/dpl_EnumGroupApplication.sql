CREATE PROCEDURE [dbo].[dpl_EnumGroupApplication]
 @GroupName as nvarchar(256)

AS
SELECT adm_Host.Name as ApplicationName
FROM adm_Host JOIN adm_Group ON adm_Host.GroupId = adm_Group.Id
WHERE (adm_Group.Name = @GroupName)
ORDER BY adm_Host.Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_EnumGroupApplication] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_EnumGroupApplication] TO [BTS_OPERATORS]
    AS [dbo];

