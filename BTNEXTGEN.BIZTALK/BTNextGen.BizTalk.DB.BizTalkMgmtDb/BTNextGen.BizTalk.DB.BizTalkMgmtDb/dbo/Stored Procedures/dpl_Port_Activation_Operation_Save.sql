CREATE PROCEDURE [dbo].[dpl_Port_Activation_Operation_Save]
(
 @OrchestrationID int,
 @PortName nvarchar(256),
 @OperationName nvarchar(256)
)

AS
set nocount on
set xact_abort on

DECLARE @PortID int, @PortTypeID int ,@OperationID int

SELECT @PortID = nID, @PortTypeID = nPortTypeID FROM bts_orchestration_port
 WHERE nOrchestrationID = @OrchestrationID AND
  nvcName = @PortName 
  
SELECT @OperationID = nID FROM bts_porttype_operation
 WHERE nPortTypeID = @PortTypeID AND
  nvcName = @OperationName 
  

INSERT INTO bts_port_activation_operation
 ( 
  nOrchestrationID,
  nPortID, 
  nOperationID 
 )
VALUES
 ( 
  @OrchestrationID, 
  @PortID, 
  @OperationID
 )
RETURN @@IDENTITY

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_Port_Activation_Operation_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

