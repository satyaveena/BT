CREATE PROCEDURE [dbo].[edi_GetValidST01Values]
	@GS01 nvarchar(50)
AS

SELECT
	[ST01]
FROM
	[dbo].[EDIX12ST01GS01Mapping]
Where
	[GS01] = @GS01
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetValidST01Values] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetValidST01Values] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetValidST01Values] TO [BTS_OPERATORS]
    AS [dbo];

