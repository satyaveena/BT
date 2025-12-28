CREATE  PROCEDURE [dbo].[bas_GetPropertyValue] @propertyName nvarchar(80), @propertyValue nvarchar(260) OUTPUT
AS
 SELECT  @propertyValue  = PropertyValue
 FROM bas_Properties
 WHERE PropertyName = @propertyName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bas_GetPropertyValue] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bas_GetPropertyValue] TO [BTS_OPERATORS]
    AS [dbo];

