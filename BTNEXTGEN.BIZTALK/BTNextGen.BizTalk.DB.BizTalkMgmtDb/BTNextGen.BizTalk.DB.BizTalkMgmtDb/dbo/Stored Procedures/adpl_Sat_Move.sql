CREATE PROCEDURE [dbo].[adpl_Sat_Move]
(
	@Luid [nvarchar] (440),
	@SdmType [nvarchar] (256),
	@AppId [int],
	@NewLuid [nvarchar] (256)	 
)
AS
set nocount on
UPDATE adpl_sat
SET
	[applicationId] = @AppId,
	[luid] = @NewLuid
WHERE	
	([sdmType]  = @SdmType) AND
	([luid]  = @Luid)
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Move] TO [BTS_ADMIN_USERS]
    AS [dbo];

