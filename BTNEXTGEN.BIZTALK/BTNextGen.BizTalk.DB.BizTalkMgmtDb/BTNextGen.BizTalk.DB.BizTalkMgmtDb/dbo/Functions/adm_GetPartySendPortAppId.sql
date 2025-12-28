CREATE FUNCTION [dbo].[adm_GetPartySendPortAppId] (@nPartySendPortID int) RETURNS int
AS
BEGIN
	declare @nAppId as int
	select @nAppId = bts_sendport.nApplicationID
		from bts_party_sendport,
			bts_sendport
		where bts_party_sendport.nID=@nPartySendPortID
			and bts_party_sendport.nSendPortID = bts_sendport.nID
		
	return @nAppId
END
