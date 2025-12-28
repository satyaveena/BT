CREATE PROCEDURE [dbo].[adpl_Sat_Load]
(
	@Luid [nvarchar] (440) 
)
AS
set nocount on
SELECT
	    [applicationId] ,
	    [luid] ,
	    [properties],
	    [files],
	    [cabContent],
	    [sdmType] 
FROM [adpl_sat] 
WHERE
	([luid]  = @Luid)
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Load] TO [BTS_OPERATORS]
    AS [dbo];

