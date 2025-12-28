CREATE FUNCTION [dbo].[adm_ValidateApplicationBinding_Sp]
(
	@appId int,
	@spId int
) RETURNS bit
AS
BEGIN
	if exists (
		select * from bts_spg_sendport where  nSendPortID = @spId and 
			dbo.adm_IsReferencedBy(dbo.adm_GetSendPortGroupAppId(nSendPortGroupID), @appId) = 0
		)
		return 0
	return 1
END
