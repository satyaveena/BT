CREATE FUNCTION [dbo].[adm_GetReferencesOfApp] (@thisAppId int ) 
RETURNS @references table (referenceId int)
AS
BEGIN
	insert @references
		select nReferencedApplicationID from bts_application_reference where nApplicationID = @thisAppId
	declare temp_Cursor cursor dynamic for 
		select referenceId from @references
	open temp_Cursor
	fetch next from temp_Cursor into @thisAppId
	while @@FETCH_STATUS = 0
	begin
		insert @references
			select nReferencedApplicationID from bts_application_reference where nApplicationID = @thisAppId
		fetch next from temp_Cursor into @thisAppId
	end
	close temp_Cursor
	deallocate temp_Cursor
	return
END
