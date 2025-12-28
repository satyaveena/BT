CREATE PROCEDURE [dbo].[trk_EnumerateReferencesRecursive]
	@moduleid int
AS
BEGIN
	IF EXISTS (SELECT moduleid FROM #Modules WHERE moduleid=@moduleid)
	RETURN
	INSERT #Modules(moduleid) VALUES(@moduleid)
	DECLARE @idlib int
	DECLARE RefCursor CURSOR LOCAL FOR SELECT idlib FROM bts_libreference WHERE idapp=@moduleid
	OPEN RefCursor
	FETCH NEXT FROM RefCursor into @idlib
	WHILE @@FETCH_STATUS<>-1 
	BEGIN
		EXEC trk_EnumerateReferencesRecursive @idlib
		FETCH NEXT FROM RefCursor into @idlib
	END
	CLOSE RefCursor
	DEALLOCATE RefCursor
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_EnumerateReferencesRecursive] TO [BTS_ADMIN_USERS]
    AS [dbo];

