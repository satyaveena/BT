CREATE PROCEDURE [dbo].[adpl_Sat_Enum_Type]
(
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
	WHERE 
	     [sdmType] = @ResourceType
	ORDER BY
	     [luid]
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_Type] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Sat_Enum_Type] TO [BTS_OPERATORS]
    AS [dbo];

