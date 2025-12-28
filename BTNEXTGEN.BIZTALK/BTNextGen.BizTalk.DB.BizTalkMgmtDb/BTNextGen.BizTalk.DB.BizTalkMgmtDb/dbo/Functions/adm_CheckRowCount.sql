CREATE FUNCTION [dbo].[adm_CheckRowCount] (@rowCount int)
RETURNS int
AS
BEGIN
 return
  case
   when @rowCount = 1 then 0   -- OKAY
   when @rowCount = 0 then 0xC0C02537 -- CIS_E_ADMIN_CORE_FAILED_INSTANCE_NOT_FOUND
   when @rowCount > 1 then 0xC0C02538 -- CIS_E_ADMIN_CORE_FAILED_TOO_MANY_INSTANCES_FOUND
  end
END