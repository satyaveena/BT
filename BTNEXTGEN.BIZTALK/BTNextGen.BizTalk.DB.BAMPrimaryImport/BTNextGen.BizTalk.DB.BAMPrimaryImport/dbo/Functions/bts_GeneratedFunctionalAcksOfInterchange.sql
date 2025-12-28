CREATE FUNCTION [dbo].[bts_GeneratedFunctionalAcksOfInterchange]
(	@InterchangeStatusActivityID nvarchar(128)
)
RETURNS TABLE 
AS
Return
(
	Select faa.ActivityID, faa.InterchangeActivityID, faa.GroupControlNo, faa.InterchangeControlNo, faa.ReceiverID, faa.SenderID, faa.ReceiverQ, faa.SenderQ, faa.InterchangeDateTime, faa.Direction, 
	faa.AckProcessingStatus, faa.AckStatusCode, faa.DeliveredTSCount, faa.AcceptedTSCount, faa.AckIcn, faa.AckIcnDate, faa.AckIcnTime, faa.ErrorCode1, faa.ErrorCode2, faa.ErrorCode3, faa.ErrorCode4,
	faa.ErrorCode5, faa.TimeCreated, faa.RowFlags, fgisa.FunctionalIDCode, fgisa.TSCount, fgisa.AgreementName
	From bam_FunctionalAckActivity_CompletedInstances faa
	inner join 
	( select isa.ReceiverID, isa.SenderID, isa.ReceiverQ, isa.SenderQ, isa.InterchangeDateTime, isa.Direction, isa.AgreementName, 
		fgi.ActivityID, fgi.InterchangeActivityID, fgi.GroupControlNo, fgi.FunctionalIDCode, fgi.TSCount
		from bam_FunctionalGroupInfo_CompletedInstances fgi
		inner join bam_InterchangeStatusActivity_CompletedInstances isa
		on fgi.InterchangeActivityID = isa.ActivityID
		where isa.ActivityID = @InterchangeStatusActivityID
	) fgisa
	on faa.InterchangeActivityID = fgisa.InterchangeActivityID AND 
	faa.GroupControlNo = fgisa.GroupControlNo AND
	faa.AckProcessingStatus = 3
)
GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_GeneratedFunctionalAcksOfInterchange] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_GeneratedFunctionalAcksOfInterchange] TO [BTS_OPERATORS]
    AS [dbo];

