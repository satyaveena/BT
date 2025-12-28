CREATE FUNCTION [dbo].[adm_GetSendPortGroupAppId] (@sendPortGroupId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = nApplicationID from bts_sendportgroup where nID=@sendPortGroupId
	return @nAppId
END
