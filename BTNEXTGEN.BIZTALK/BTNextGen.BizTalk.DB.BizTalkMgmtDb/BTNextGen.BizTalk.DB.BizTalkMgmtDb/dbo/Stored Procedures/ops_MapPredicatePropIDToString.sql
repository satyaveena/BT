CREATE PROCEDURE [dbo].[ops_MapPredicatePropIDToString]
@nPropertiesCount int OUTPUT
AS

set transaction isolation level read committed
set nocount on
set deadlock_priority low

 SELECT id, msgtype 
 FROM bt_DocumentSpec 
 WHERE is_property_schema = 1

 set @nPropertiesCount = @@ROWCOUNT
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_MapPredicatePropIDToString] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ops_MapPredicatePropIDToString] TO [BTS_OPERATORS]
    AS [dbo];

