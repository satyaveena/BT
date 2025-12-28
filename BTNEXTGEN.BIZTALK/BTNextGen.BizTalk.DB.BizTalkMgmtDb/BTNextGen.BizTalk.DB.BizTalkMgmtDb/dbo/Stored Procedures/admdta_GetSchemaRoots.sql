CREATE PROCEDURE [dbo].[admdta_GetSchemaRoots]
AS 
	declare @nonlocalized_string_property_schema AS nvarchar(256)
	set @nonlocalized_string_property_schema  = N'@#%&<Property Schema>&%#@' -- this is hardcoded value that is replaced with localized value in the UI
	SELECT 	-- documents
		docs.docspec_name AS DocSpecName, 
		docs.schema_root_name AS SchemaRootName, 
		docs.msgtype AS MsgType,
		docs.assemblyid AS AssemblyID,
		docs.is_tracked AS TrackAll,
		docs.id	AS ID,
		asm.nvcFullName AS AssemblyFullName,
		docs.itemid as ItemId
	FROM 	dbo.bt_DocumentSpec docs
	INNER JOIN dbo.bt_XMLShare shares  ON docs.shareid = shares.id,
			dbo.bts_assembly asm
	WHERE	docs.is_property_schema = 0 AND
			asm.nID = docs.assemblyid
	UNION ALL
	SELECT 	-- property schemas
		docs.docspec_name AS DocSpecName, 
		@nonlocalized_string_property_schema AS SchemaRootName,
		shares.target_namespace AS MsgType,
		docs.assemblyid AS AssemblyID,
		CAST(0 as bit) as TrackAll,
		shares.id AS ID,
		asm.nvcFullName AS AssemblyFullName,
		MIN(docs.itemid) as ItemId -- It doesn't really matter which one we pick, they all the same
	FROM 	dbo.bt_DocumentSpec docs
		INNER JOIN dbo.bt_XMLShare shares  ON docs.shareid = shares.id,
		dbo.bts_assembly asm
	WHERE	docs.is_property_schema <> 0 AND
		asm.nID = docs.assemblyid
	GROUP BY docs.docspec_name, shares.target_namespace, docs.assemblyid, shares.id, asm.nvcFullName
	ORDER BY DocSpecName ASC, SchemaRootName ASC

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_GetSchemaRoots] TO [BTS_ADMIN_USERS]
    AS [dbo];

