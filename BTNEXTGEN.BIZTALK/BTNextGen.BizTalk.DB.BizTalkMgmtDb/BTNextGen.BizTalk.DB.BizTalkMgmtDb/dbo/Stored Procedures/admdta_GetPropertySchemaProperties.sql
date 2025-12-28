CREATE PROCEDURE [dbo].[admdta_GetPropertySchemaProperties] 
@msgType nvarchar(256),
@itemid  int
AS
	SELECT 	
		docs.schema_root_name AS Name,
		docs.is_tracked AS IsTracked,
		docs.id
	FROM  	dbo.bt_DocumentSpec docs
	WHERE 	
		docs.itemid = @itemid
	ORDER BY Name ASC, is_tracked ASC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetPropertySchemaProperties] TO [BTS_ADMIN_USERS]
    AS [dbo];

