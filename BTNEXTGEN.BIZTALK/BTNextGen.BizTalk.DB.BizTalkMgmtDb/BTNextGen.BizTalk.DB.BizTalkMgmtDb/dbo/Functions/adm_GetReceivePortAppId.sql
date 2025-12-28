CREATE FUNCTION [dbo].[adm_GetReceivePortAppId] (@receivePortId int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = nApplicationID 
		from bts_receiveport 
		where nID=@receivePortId
	return @nAppId
END
