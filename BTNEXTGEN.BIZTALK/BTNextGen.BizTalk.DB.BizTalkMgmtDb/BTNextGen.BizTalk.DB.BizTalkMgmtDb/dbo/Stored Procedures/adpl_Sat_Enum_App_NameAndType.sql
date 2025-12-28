CREATE PROCEDURE [dbo].[adpl_Sat_Enum_App_NameAndType]
(
	@ApplicationName as [nvarchar] (256),
	@ResourceType as [nvarchar] (256)
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
	     bta.nvcName = @ApplicationName AND [sdmType] = @ResourceType
	ORDER BY
	     [luid]
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_NameAndType] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_App_NameAndType] TO [BTS_OPERATORS]
    AS [dbo];

