CREATE PROCEDURE [dbo].[dpl_Orchestration_Port_Save]
(
 @ServiceID as int,
 @PortGuid as nvarchar(256),
 @Name as nvarchar(256),
 @PortTypeName as nvarchar(256),
 @PortTypeNamespace as nvarchar(256),
 @PortTypeAssemblyName as nvarchar(256),
 @Polarity as int,
 @BindingOption as int,
 @IsLink as int,
 @RoleID as int 
)

AS
set nocount on
set xact_abort on

DECLARE @porttypeid AS INT
DECLARE @roleporttypeid AS INT

SELECT @porttypeid = porttype.nID 
 FROM bts_porttype porttype
  INNER JOIN bts_assembly assembly ON porttype.nAssemblyID = assembly.nID
 WHERE porttype.nvcNamespace = @PortTypeNamespace AND
       porttype.nvcName = @PortTypeName AND
       assembly.nvcFullName = @PortTypeAssemblyName

IF ( @porttypeid IS NULL )
 RETURN -1

IF ( @IsLink > 0 )
BEGIN
 SELECT @roleporttypeid = nID 
  FROM bts_role_porttype
  WHERE nRoleID = @RoleID AND
   nPortTypeID = @porttypeid 

 IF ( @roleporttypeid IS NULL )
  RETURN -2
END

INSERT INTO bts_orchestration_port
 ( 
  uidGUID,
  nOrchestrationID,
  nPortTypeID,
  nvcName,
  nPolarity,
  nBindingOption,
  nRolePortTypeID,
  bLink
 )
VALUES
 (
  convert(uniqueidentifier,@PortGuid),
  @ServiceID,
  @porttypeid,
  @Name, 
  @Polarity, -- 1 = inbound, 2 = outbound
  @BindingOption,
   -- 1 = Logical (default)
   -- 2 = Physical
   -- 3 = Direct (Not available for binding thru Explorer)
   -- 4 = Dynamic
   -- 5 = ServiceLinkType
  @roleporttypeid,
  @IsLink 
 )
RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Orchestration_Port_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

