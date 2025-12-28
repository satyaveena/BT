CREATE FUNCTION [dbo].[adm_ValidateApplicationBinding_Spg]
(
	@appId int,
	@spgId int
) RETURNS bit
AS
BEGIN
	if exists (
		select * from bts_spg_sendport where  nSendPortGroupID = @spgId and 
			dbo.adm_IsReferencedBy(@appId, dbo.adm_GetSendPortAppId(nSendPortID)) = 0
		)
		return 0
	return 1
END
