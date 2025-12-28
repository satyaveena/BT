CREATE FUNCTION [dbo].[admsvr_CheckForMapOnReceivePort] (@receivePortID int)
RETURNS uniqueidentifier
AS
BEGIN
	return
		(SELECT TOP 1 uidTransformGUID
		FROM bts_receiveport_transform rptrans
		INNER JOIN bts_receiveport rp ON rp.nID = rptrans.nReceivePortID
		WHERE @receivePortID = rp.nID)
END
