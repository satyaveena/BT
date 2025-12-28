CREATE PROCEDURE [dbo].[admdta_SetSchemaRootTrackAllProperty]
@rootId uniqueidentifier,
@tracked bit
 AS 
	set nocount on
	declare @ErrCode as int
	select @ErrCode=0
	update 	dbo.bt_DocumentSpec 
		set 	is_tracked=@tracked
		where 	id=@rootId
	set @ErrCode = dbo.adm_CheckRowCount(@@ROWCOUNT)
	
	set nocount off
	return @ErrCode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admdta_SetSchemaRootTrackAllProperty] TO [BTS_ADMIN_USERS]
    AS [dbo];

