CREATE FUNCTION [dbo].[adm_GetSendPortAppId] (@sendPortId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = nApplicationID from bts_sendport where nID=@sendPortId
	return @nAppId
END
