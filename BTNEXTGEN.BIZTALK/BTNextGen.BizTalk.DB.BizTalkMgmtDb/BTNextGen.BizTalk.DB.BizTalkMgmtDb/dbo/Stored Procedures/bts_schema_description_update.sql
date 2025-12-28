create procedure [dbo].[bts_schema_description_update]
@itemid int,
@docspec_name nvarchar(513),
@description nvarchar(1024)
AS
set nocount on
declare @property int
select @property = is_property_schema from bt_DocumentSpec where itemid = @itemid
if (@property = 1)
begin
	update bts_item
	set description = @description
	where id = @itemid
end
else
begin
	update bt_DocumentSpec
	set description = @description
	where itemid = @itemid AND docspec_name = @docspec_name
end
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_schema_description_update] TO [BTS_ADMIN_USERS]
    AS [dbo];

