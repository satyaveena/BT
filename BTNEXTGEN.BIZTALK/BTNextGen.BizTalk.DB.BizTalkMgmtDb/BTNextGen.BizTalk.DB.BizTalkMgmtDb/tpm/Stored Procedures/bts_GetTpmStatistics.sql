CREATE PROCEDURE [tpm].[bts_GetTpmStatistics]
	@nHostInstanceId uniqueidentifier,
	@nAS2AgreementCount int output,
	@nEDIFACTAgreementCount int output,
	@nPartyCount int output,
	@nHostPartyCount int output,
	@nMaxProfiles int output,
	@nX12AgreementCount int output,
	@nProtocolSettingsCount int output,
	@nEDIPartyCount int output
AS
	declare @nLowestHostInstanceId uniqueidentifier
	--check if @nHostId is the lowest host id
	select top 1 @nLowestHostInstanceId = [dbo].[adm_HostInstance].UniqueId from [dbo].[adm_HostInstance] with (NOLOCK)
		join [dbo].[adm_Server2HostMapping] with (NOLOCK) on [dbo].[adm_HostInstance].Svr2HostMappingId = [dbo].[adm_Server2HostMapping].Id
		join [dbo].[adm_Host] with (NOLOCK) on [dbo].[adm_Server2HostMapping].HostId = [dbo].[adm_Host].Id
		where [dbo].[adm_Host].HostType = 1 order by [dbo].[adm_HostInstance].Id asc
	--only proceed if the host instance passed is the one with the lowest id
	if (@nHostInstanceId != @nLowestHostInstanceId)
		return -1
	--AS2 Agreement Count
	select @nAS2AgreementCount = COUNT(*) from [tpm].[Agreement] with (NOLOCK) Where Protocol = 'as2'
	--EDIFACT Agreement Count
	select @nEDIFACTAgreementCount = COUNT(*) from [tpm].[Agreement] with (NOLOCK) Where Protocol = 'edifact'
	--Number of Parties
	select @nPartyCount = COUNT(*) from [tpm].[Partner] with (NOLOCK)
	
	--Number of Parties having multiple Partnerships
	select @nHostPartyCount = COUNT(*) from
					(Select PartnerId
					 From (Select PartnerAId PartnerId From tpm.Partnership with (NOLOCK) union all Select PartnerBId PartnerId From tpm.Partnership with (NOLOCK)) PartiesHavingPartnerships
					 Group by PartnerId
					 Having COUNT(PartnerId) > 1) PartiesHavingMultiplePartnerships
	--Max Profiles in any Party
	select @nMaxProfiles = ISNULL(MAX(ProfileCount),0) From
						(Select PartnerId, COUNT(ProfileId) ProfileCount
						 From tpm.BusinessProfile with (NOLOCK)
						 Group by PartnerId) ProfilesGroupedByPartner
	--Number of X12 Agreements
	select @nX12AgreementCount = COUNT(*) from [tpm].[Agreement] with (NOLOCK) Where Protocol = 'x12'
	--Number of times Protocol Settings is used in Profiles
	select @nProtocolSettingsCount = COUNT(*) from [tpm].[ProtocolSettings] with (NOLOCK) Where ProfileId is not null
	--Number of Parties involved in EDI/AS2
	select @nEDIPartyCount = Count(*) from (Select PartnerAId From tpm.Partnership with (NOLOCK) union Select PartnerBId PartnerId From tpm.Partnership with (NOLOCK)) PartiesHavingPartnerships
	
	return 0

GO
GRANT EXECUTE
    ON OBJECT::[tpm].[bts_GetTpmStatistics] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[tpm].[bts_GetTpmStatistics] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[tpm].[bts_GetTpmStatistics] TO [BTS_B2B_OPERATORS]
    AS [dbo];

