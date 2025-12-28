create procedure [dbo].[TDDS_CheckAdminDBVersion]
(
 @ClientVersion nvarchar(128), 
 @AdminDBVersion nvarchar(128) OUTPUT
)
as
begin
	SET @AdminDBVersion = N'1.0.0'
	return 0
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_CheckAdminDBVersion] TO [BTS_ADMIN_USERS]
    AS [dbo];

