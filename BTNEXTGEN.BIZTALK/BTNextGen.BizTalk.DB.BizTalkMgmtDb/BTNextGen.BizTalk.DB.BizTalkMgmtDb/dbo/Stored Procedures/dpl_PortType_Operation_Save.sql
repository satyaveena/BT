CREATE PROCEDURE [dbo].[dpl_PortType_Operation_Save]
(
 @PortTypeID int,
 @Name nvarchar(256),
 @Type int
)

AS
set nocount on
set xact_abort on

INSERT INTO bts_porttype_operation
 ( 
  nPortTypeID,
  nvcName, 
  nType 
 )
VALUES
 ( 
  @PortTypeID, 
  @Name, 
  @Type
 )
RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_PortType_Operation_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

