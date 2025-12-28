CREATE PROCEDURE [dbo].[dpl_MessageType_Part_Save]
(
 @MessageTypeID as int,
 @Name as nvarchar(256),
 @Namespace as nvarchar(256),
 @SchemaURTNamespace nvarchar(256),
 @SchemaURTTypename nvarchar(256)
)

AS
set nocount on
set xact_abort on

  INSERT INTO bts_messagetype_part
   ( 
    nMessageTypeID,
    nvcName,
    nvcNamespace,
    nvcSchemaURTNameSpace,
    nvcSchemaURTTypeName
   )
  VALUES
   (
    @MessageTypeID,
    @Name,
    @Namespace,
    @SchemaURTNamespace,
    @SchemaURTTypename
   )

RETURN 0

set nocount off
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[dpl_MessageType_Part_Save] TO [BTS_ADMIN_USERS]
    AS [dbo];

