CREATE PROCEDURE [dbo].[trk_GetServicesPerAssembly]
	@ModuleName nvarchar(256)
AS
BEGIN
	DECLARE @@moduleID int
	SELECT @@moduleID=nID FROM bts_assembly WHERE bts_assembly.nvcFullName=@ModuleName	
	SELECT bts_orchestration.nvcFullName FROM bts_orchestration
	WHERE nAssemblyID=@@moduleID
	CREATE TABLE #Modules (moduleid int)
	EXEC trk_EnumerateReferencesRecursive @@moduleID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_GetServicesPerAssembly] TO [BTS_ADMIN_USERS]
    AS [dbo];

