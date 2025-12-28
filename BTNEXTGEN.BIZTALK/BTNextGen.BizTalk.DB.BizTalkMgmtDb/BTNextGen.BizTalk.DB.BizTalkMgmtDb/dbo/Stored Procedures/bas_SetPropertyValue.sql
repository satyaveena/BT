CREATE PROCEDURE [dbo].[bas_SetPropertyValue] @propertyName nvarchar(80), @propertyValue nvarchar(260)
AS
 IF (EXISTS(SELECT * FROM bas_Properties WHERE PropertyName = @propertyName))
  BEGIN
   UPDATE bas_Properties
   SET PropertyValue = @propertyValue
   WHERE PropertyName = @propertyName
  END
 ELSE
  BEGIN
   INSERT INTO bas_Properties (PropertyName, PropertyValue)
   VALUES (@propertyName, @propertyValue)
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bas_SetPropertyValue] TO [BTS_ADMIN_USERS]
    AS [dbo];

