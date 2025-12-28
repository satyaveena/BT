CREATE PROCEDURE [dbo].[edi_LoadAdapterAliases] 

AS

SELECT 
	Adapters.Name,
	Aliases.AliasValue as Alias
FROM
	[dbo].[adm_Adapter] as Adapters
	LEFT JOIN [adm_AdapterAlias] as Aliases ON Adapters.Id = Aliases.AdapterId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_LoadAdapterAliases] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_LoadAdapterAliases] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_LoadAdapterAliases] TO [BTS_OPERATORS]
    AS [dbo];

