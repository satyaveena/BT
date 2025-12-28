CREATE PROCEDURE [dbo].[adpl_Sat_Enum_App_Id]
(
	@ApplicationId as int
)
AS
SET NOCOUNT ON
SELECT
            [sdmType],
            [luid],
            [properties],
            [files],
            [cabContent] 
	FROM [adpl_sat]
	WHERE 
	     [applicationId] = @ApplicationId
	ORDER BY
	    [sdmType], [luid]
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_Id] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_Id] TO [BTS_OPERATORS]
    AS [dbo];

