CREATE PROCEDURE [dbo].[dpl_RoleLinkType_Save_Role]
(
 @ServiceLinkTypeID as int,
 @Name as nvarchar(256)
)

AS
set nocount on
set xact_abort on

  INSERT INTO bts_role
   ( 
    nRoleLinkTypeID,
    nvcName
   )
  VALUES
   (
    @ServiceLinkTypeID,
    @Name
   )

RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_RoleLinkType_Save_Role] TO [BTS_ADMIN_USERS]
    AS [dbo];

