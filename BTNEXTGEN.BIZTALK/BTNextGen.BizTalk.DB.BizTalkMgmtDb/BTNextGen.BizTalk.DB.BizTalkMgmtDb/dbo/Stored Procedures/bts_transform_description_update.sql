create procedure [dbo].[bts_transform_description_update]
@id uniqueidentifier,
@description nvarchar(1024)
AS
set nocount on
update bt_MapSpec
set description = @description
where id = @id
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_transform_description_update] TO [BTS_ADMIN_USERS]
    AS [dbo];

