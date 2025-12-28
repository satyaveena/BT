CREATE PROCEDURE [dbo].[TDDS_GetColumnInfo]
	@objectName	nvarchar(256)
AS

DECLARE @id as int

SELECT @id=id FROM sysobjects 
WHERE 
id = object_id(@objectName)

if (@@rowcount =0) 
begin
	declare @localized_string_error60050 as nvarchar(128)
	set @localized_string_error60050 = N'Stored Procedure Does Not Exist.'
	raiserror(@localized_string_error60050, 16, 1)
	return 60050
end
	
select
	'Column_name'	= name,
	'Type'		= xtype,
	'Length'	= convert(int, length)

from syscolumns 
where id=@id
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [BAM_EVENT_WRITER]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_ResendJournalActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_ResendTrackingActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_InterchangeStatusActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_FunctionalGroupInfo_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_InterchangeAckActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_FunctionalAckActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_BatchingActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_BatchInterchangeActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_TransactionSetActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_BusinessMessageJournal_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_AS2InterchangeActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_AS2MessageActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_AS2MdnActivity_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_EmailFromFile_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_APIDemo_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_active_credit_cards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_Cybersource Settlement Feed_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_ERPOrders_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_ExpiredCreditCards_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_FirstDataRecon_EventWriter]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetColumnInfo] TO [bam_PaymentERPTxn_EventWriter]
    AS [dbo];

