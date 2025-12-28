CREATE PROCEDURE [dbo].[edi_GetPropertyTypeInfoFromClrType]
(
    @clrns nvarchar(256),
    @rootName nvarchar(2000)
)
AS

select msgtype, xsd_type from [dbo].[bt_DocumentSpec] where clr_namespace = @clrns and schema_root_name = @rootName and is_property_schema=1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromClrType] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromClrType] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetPropertyTypeInfoFromClrType] TO [BTS_OPERATORS]
    AS [dbo];

