CREATE PROCEDURE [dbo].[trk_GetServiceID] 
(
	@ModuleName nvarchar(256),
	@ServiceName nvarchar(256)
)
AS
BEGIN
	SELECT uidGUID
	FROM bts_orchestration o
	INNER JOIN bts_assembly a ON a.nID = o.nAssemblyID
	WHERE a.nvcFullName = @ModuleName 
	AND o.nvcFullName = @ServiceName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_GetServiceID] TO [BTS_ADMIN_USERS]
    AS [dbo];

