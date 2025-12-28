CREATE    PROCEDURE [dbo].[ediInt_PropertyListSelect]
AS
SELECT 
	msgtype, 
	clr_namespace,
	clr_namespace + '.' + schema_root_name as shortName,
	schema_root_name,
	xsd_type
FROM
	[dbo].[bt_DocumentSpec]
WHERE
	xsd_type <> ''
ORDER BY 
	clr_namespace + schema_root_name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ediInt_PropertyListSelect] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ediInt_PropertyListSelect] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ediInt_PropertyListSelect] TO [BTS_OPERATORS]
    AS [dbo];

