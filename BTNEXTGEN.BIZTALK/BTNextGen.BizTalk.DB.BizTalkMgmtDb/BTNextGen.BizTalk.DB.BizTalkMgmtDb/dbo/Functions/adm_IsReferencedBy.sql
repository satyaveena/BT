CREATE FUNCTION [dbo].[adm_IsReferencedBy]
(
	@thisAppId int,
	@tobeAddedAppId int
) RETURNS bit
AS
BEGIN
	if(IsNull(@thisAppId, N'') = N'')
		return 1
	if(IsNull(@tobeAddedAppId, N'') = N'')
		return 1
	if @thisAppId = @tobeAddedAppId
		return 1
	-- Search references
	if exists( select referenceId from dbo.adm_GetReferencesOfApp(@thisAppId)
		where referenceId = @tobeAddedAppId )
	begin
		return 1
	end
	return 0
END
