CREATE PROCEDURE [dbo].[adpl_Sat_Delete]
(
	@SdmType [nvarchar] (256),
	@Luid [nvarchar] (440) 
)
AS
set nocount on
DELETE
	[adpl_sat] 
WHERE
	([sdmType]  = @SdmType) AND
	([luid]  = @Luid)
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Delete] TO [BTS_ADMIN_USERS]
    AS [dbo];

