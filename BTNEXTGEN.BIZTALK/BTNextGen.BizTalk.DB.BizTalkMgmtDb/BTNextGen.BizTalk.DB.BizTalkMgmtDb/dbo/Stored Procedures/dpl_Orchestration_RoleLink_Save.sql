CREATE PROCEDURE [dbo].[dpl_Orchestration_RoleLink_Save]
(
 @Name as nvarchar(256),
 @ServiceID as int,
 @ServiceLinkTypeName as nvarchar(256),
 @ServiceLinkTypeNamespace as nvarchar(256),
 @RoleName as nvarchar(256),
 @ALTAssemblyName as nvarchar(256),
 @Implements as int,
 @BindingType as int,
 @RoleID as int OUTPUT
)

AS
set nocount on
set xact_abort on


SELECT @RoleID = rl.nID 
 FROM bts_role rl
  INNER JOIN bts_rolelink_type slt ON rl.nRoleLinkTypeID = slt.nID
  INNER JOIN bts_assembly assembly ON slt.nAssemblyID = assembly.nID
 WHERE slt.nvcNamespace = @ServiceLinkTypeNamespace AND
       slt.nvcName = @ServiceLinkTypeName AND
       rl.nvcName = @RoleName AND
       assembly.nvcFullName = @ALTAssemblyName

IF ( @RoleID IS NULL )
 RETURN -1


INSERT INTO bts_rolelink
 ( 
  nvcName,
  nOrchestrationID,
  nRoleID,
  bImplements,
  nBindingType
 )
VALUES
 (
  @Name, 
  @ServiceID,
  @RoleID,
  @Implements,
  @BindingType
   -- 1 = Logical (default)
   -- 2 = Physical
   -- 3 = Direct (Not available for binding thru Explorer)
   -- 4 = Dynamic
 )
RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_RoleLink_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

