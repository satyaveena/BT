CREATE PROCEDURE [dbo].[adpl_Application_Load]
(
	@Name AS nvarchar(512),
	@Id AS int OUTPUT,
	@IsDefault AS int OUTPUT,
	@IsSystem AS int OUTPUT,
	@Description as nvarchar(1024) OUTPUT,
	@DateModified as datetime OUTPUT
)
AS
SET NOCOUNT ON
SELECT
            @Id = [nID],
            @IsDefault = [isDefault],
            @IsSystem = [isSystem],
            @Description = [nvcDescription],
            @DateModified = [DateModified] 
	FROM [bts_application]
	WHERE 
	     [nvcName] = @Name
	     
SET NOCOUNT OFF

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Application_Load] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[adpl_Application_Load] TO [BTS_OPERATORS]
    AS [dbo];

