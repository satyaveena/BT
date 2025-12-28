CREATE PROCEDURE [dbo].[trk_GetSchema] 
	@ModuleName nvarchar(256),
	@TypeName nvarchar(256)
AS
BEGIN
	DECLARE @@moduleID int
	SELECT @@moduleID=nID FROM bts_assembly WHERE bts_assembly.nvcFullName=@ModuleName	
	CREATE TABLE #Modules (moduleid int)
	EXEC trk_EnumerateReferencesRecursive @@moduleID
	SELECT Top 1 content FROM bt_XMLShare
	JOIN bt_DocumentSpec ON bt_XMLShare.id=bt_DocumentSpec.shareid
	JOIN #Modules ON bt_DocumentSpec.assemblyid=#Modules.moduleid
	WHERE ( (schema_root_clr_fqn=@TypeName) or (schema_root_clr_fqn LIKE @TypeName+'+%'))
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_GetSchema] TO [BTS_ADMIN_USERS]
    AS [dbo];

