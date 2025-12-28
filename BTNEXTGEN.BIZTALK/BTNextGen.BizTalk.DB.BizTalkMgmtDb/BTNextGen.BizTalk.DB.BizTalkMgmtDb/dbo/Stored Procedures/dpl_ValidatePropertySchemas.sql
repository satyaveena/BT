CREATE PROCEDURE [dbo].[dpl_ValidatePropertySchemas]

AS

  -- Now verify and update references from bt_Properties to bt_DocumentSpec - no document property might be orphaned by 
  -- property schema downgrade
  DECLARE @orphancount int
  SELECT @orphancount =  dbo.dpl_fn_CountOrphanedProperties()
  IF ( @orphancount > 0 )
   RETURN -2

RETURN 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_ValidatePropertySchemas] TO [BTS_ADMIN_USERS]
    AS [dbo];

