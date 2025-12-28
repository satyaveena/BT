CREATE PROCEDURE [dbo].[admdta_GetSchemaRootProperties] 
@msgType nvarchar(256),
@itemid  int
AS
			SELECT 
				name AS Name,
				is_tracked AS IsTracked,
				id
	
			FROM  	dbo.bt_Properties
			WHERE	itemid = @itemid AND msgtype=@msgType
			ORDER BY name ASC, is_tracked ASC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetSchemaRootProperties] TO [BTS_ADMIN_USERS]
    AS [dbo];

