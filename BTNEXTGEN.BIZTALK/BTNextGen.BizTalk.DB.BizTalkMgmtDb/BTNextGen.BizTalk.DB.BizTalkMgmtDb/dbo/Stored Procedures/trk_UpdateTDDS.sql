CREATE PROCEDURE [dbo].[trk_UpdateTDDS] 
(
	@DestinationName nvarchar(256),
	@ConnectionString nvarchar(1024)
)
AS
BEGIN
	UPDATE TDDS_Destinations
	SET ConnectionString = @ConnectionString
	WHERE DestinationName = @DestinationName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[trk_UpdateTDDS] TO [BTS_ADMIN_USERS]
    AS [dbo];

