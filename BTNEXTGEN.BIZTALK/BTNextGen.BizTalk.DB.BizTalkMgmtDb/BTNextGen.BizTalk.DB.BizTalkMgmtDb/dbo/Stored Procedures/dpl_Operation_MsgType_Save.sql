CREATE PROCEDURE [dbo].[dpl_Operation_MsgType_Save]
(
 @OperationID int,
 @MsgTypeName nvarchar(256),
 @MsgTypeNamespace nvarchar(256),
 @MsgTypeAssemblyName nvarchar(256),
 @Type int
)

AS
set nocount on
set xact_abort on

DECLARE @MessageTypeID int
SELECT @MessageTypeID =  msgtype.nID FROM bts_messagetype msgtype
  INNER JOIN bts_assembly assembly ON msgtype.nAssemblyID = assembly.nID
 WHERE msgtype.nvcNamespace = @MsgTypeNamespace AND
  msgtype.nvcName = @MsgTypeName AND
  assembly.nvcFullName = @MsgTypeAssemblyName
  
IF ( @MessageTypeID IS NULL )
 RETURN -1

INSERT INTO bts_operation_msgtype
 ( 
  nOperationID,
  nMessageTypeID, 
  nType 
 )
VALUES
 ( 
  @OperationID, 
  @MessageTypeID, 
  @Type
 )
RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Operation_MsgType_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

