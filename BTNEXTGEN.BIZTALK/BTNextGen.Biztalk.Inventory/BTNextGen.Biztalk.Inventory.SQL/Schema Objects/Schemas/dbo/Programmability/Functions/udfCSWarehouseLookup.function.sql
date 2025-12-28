CREATE FUNCTION [dbo].[udfCSWarehouseLookup]
(
	@ERPWarehouse CHAR(3)
)
RETURNS varchar(15)
AS
BEGIN
	Declare @Return as varchar(15)
	SELECT @Return = CSWareHouse  FROM CSWarehouseLookup with (nolock) where ERPWareHouse = @ERPWarehouse
	return @Return
END