CREATE PROCEDURE [dbo].[dpl_RoleLinkType_Save_Role_PortType]
(
 @RoleID as int,
 @PortTypeName as nvarchar(256),
 @PortTypeNamespace as nvarchar(256),
 @PortTypeAssemblyName nvarchar(256)
)

AS
 set nocount on
 set xact_abort on
 
 DECLARE @PortTypeID int
 SELECT @PortTypeID = porttype.nID FROM bts_porttype porttype
   INNER JOIN bts_assembly assembly ON porttype.nAssemblyID = assembly.nID
  WHERE porttype.nvcNamespace = @PortTypeNamespace AND
        porttype.nvcName = @PortTypeName AND
        assembly.nvcFullName = @PortTypeAssemblyName
 
 IF ( @PortTypeID IS NULL )
  RETURN -1
 
 INSERT INTO bts_role_porttype
  ( 
   nRoleID,
   nPortTypeID
  )
 VALUES
  (
   @RoleID,
   @PortTypeID
  )
 
RETURN 1

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_RoleLinkType_Save_Role_PortType] TO [BTS_ADMIN_USERS]
    AS [dbo];

