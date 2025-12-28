CREATE  PROCEDURE [dbo].[edi_GetControlMessage]
AS
BEGIN TRANSACTION
delete from [dbo].[PAM_Control] where UsedOnce = 1
select *, 1 as ToBeBatched from [dbo].[PAM_Control] PAM_Control where UsedOnce = 0 for xml auto, elements
update [dbo].[PAM_Control] set UsedOnce = 1 where UsedOnce = 0
COMMIT TRANSACTION
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetControlMessage] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetControlMessage] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[edi_GetControlMessage] TO [BTS_OPERATORS]
    AS [dbo];

