CREATE  PROCEDURE [dbo].[bas_DeleteProperty] @propertyName nvarchar(80)
AS
 DELETE FROM bas_Properties
 WHERE PropertyName = @propertyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bas_DeleteProperty] TO [BTS_ADMIN_USERS]
    AS [dbo];

