CREATE PROCEDURE [dbo].[adpl_Sat_Enum_App_Name]
(
	@ApplicationName as [nvarchar] (256)
)
AS
SET NOCOUNT ON
SELECT
            [sdmType],
            [luid],
            [properties],
            [files],
            [cabContent] 
	FROM [adpl_sat] sat
	JOIN [bts_application] bta ON sat.applicationId = bta.nID
	WHERE 
	     bta.nvcName = @ApplicationName
	ORDER BY
	    [sdmType], [luid]
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_Name] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_Name] TO [BTS_OPERATORS]
    AS [dbo];

