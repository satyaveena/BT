CREATE PROCEDURE [dbo].[adpl_Sat_Load_App]
(
	@Luid [nvarchar] (440),
	@AppId as int
)
AS
set nocount on
SELECT
	    [applicationId] ,
	    [luid],
	    [properties],
	    [files],
	    [cabContent],
	    [sdmType] 
FROM [adpl_sat] 
WHERE
	([luid]  = @Luid) AND
	([applicationId]  = @AppId)
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Load_App] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Load_App] TO [BTS_OPERATORS]
    AS [dbo];

