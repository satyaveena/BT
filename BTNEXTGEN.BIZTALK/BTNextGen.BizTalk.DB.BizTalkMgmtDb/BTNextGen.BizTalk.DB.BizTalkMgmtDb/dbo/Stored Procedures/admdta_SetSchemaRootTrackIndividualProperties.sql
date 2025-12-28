CREATE PROCEDURE [dbo].[admdta_SetSchemaRootTrackIndividualProperties]
@propertyId uniqueidentifier,
@tracked bit
 AS 
	set nocount on
	declare @ErrCode as int
	select @ErrCode=0
	update 	dbo.bt_Properties
		set 	is_tracked=@tracked
		where 	id=@propertyId
	-- This is probably a property schema
	if (@@ROWCOUNT = 0)
	begin
		update 	dbo.bt_DocumentSpec
			set 	is_tracked=@tracked
			where 	id=@propertyId
	end
	set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
	
	set nocount off
	return @ErrCode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_SetSchemaRootTrackIndividualProperties] TO [BTS_ADMIN_USERS]
    AS [dbo];

