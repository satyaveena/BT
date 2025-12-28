CREATE PROCEDURE [dbo].[bts_assembly_load]
(
	@NvcFullName [nvarchar] (256)
)
AS
set nocount on
SELECT
	    [nvcFullName] ,
	    [nSystemAssembly]
FROM [bts_assembly] 
WHERE
	([nvcFullName]  = @NvcFullName)
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_assembly_load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_assembly_load] TO [BTS_OPERATORS]
    AS [dbo];

