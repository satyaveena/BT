CREATE PROCEDURE [dbo].[edi_GetPropertyTypeInfoFromMsgType]
(
    @msgtype nvarchar(2000)
)
AS

select  schema_root_name, xsd_type from [dbo].[bt_DocumentSpec] where msgtype = @msgtype and is_property_schema=1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromMsgType] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromMsgType] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromMsgType] TO [BTS_OPERATORS]
    AS [dbo];

