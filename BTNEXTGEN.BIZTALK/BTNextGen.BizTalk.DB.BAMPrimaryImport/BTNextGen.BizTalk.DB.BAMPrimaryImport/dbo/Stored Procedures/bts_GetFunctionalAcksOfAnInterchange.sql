CREATE PROCEDURE [dbo].[bts_GetFunctionalAcksOfAnInterchange]
(
	@InterchangeStatusActivityID nvarchar(128),
	@direction int
) AS
BEGIN
	if (@direction = 2) 	begin
		Select faa.GroupControlNo, faa.SenderQ, faa.SenderID, faa.ReceiverQ, faa.ReceiverID, fgisa.AgreementName, 
			faa.Direction, faa.AckProcessingStatus, faa.AckStatusCode, 
			fgisa.FunctionalIDCode FunctionalIDCode, fgisa.TSCount, 
			faa.AckIcn, faa.AckIcnDate, faa.AckIcnTime, faa.DeliveredTSCount,  faa.AcceptedTSCount, faa.ErrorCode1, faa.ErrorCode2, 
			faa.ErrorCode3, faa.ErrorCode4, faa.ErrorCode5, faa.TimeCreated
		From bam_FunctionalAckActivity_CompletedInstances faa
		inner join 
		( 
			select isa.SenderQ, isa.SenderID, isa.ReceiverQ, isa.ReceiverID, isa.AgreementName, isa.Direction, fgi.InterchangeActivityID, fgi.GroupControlNo, fgi.FunctionalIDCode, fgi.TSCount
			from bam_FunctionalGroupInfo_CompletedInstances fgi
			inner join bam_InterchangeStatusActivity_CompletedInstances isa
			on fgi.InterchangeActivityID = isa.ActivityID
			where isa.ActivityID = @InterchangeStatusActivityID
		) fgisa
		on faa.ReceiverID = fgisa.SenderID AND faa.SenderID = fgisa.ReceiverID AND 
			faa.ReceiverQ = fgisa.SenderQ AND faa.SenderQ = fgisa.ReceiverQ AND 
			faa.GroupControlNo = fgisa.GroupControlNo
			AND faa.Direction <> fgisa.Direction
	end
	else
	begin
		Select GroupControlNo, SenderQ, SenderID, ReceiverQ, ReceiverID, AgreementName, Direction, AckProcessingStatus, AckStatusCode, 
			FunctionalIDCode, TSCount, AckIcn, AckIcnDate, AckIcnTime, DeliveredTSCount,  AcceptedTSCount, 
			ErrorCode1, ErrorCode2, ErrorCode3, ErrorCode4, ErrorCode5, [TimeCreated]
			from bts_SentFunctionalAcksOfInterchange(@InterchangeStatusActivityID) as faa
		UNION ALL
		(
			select allTable.GroupControlNo, allTable.SenderQ, allTable.SenderID, allTable.ReceiverQ, allTable.ReceiverID, allTable.AgreementName, 
				allTable.Direction, allTable.AckProcessingStatus, allTable.AckStatusCode, allTable.FunctionalIDCode, 
				allTable.TSCount, allTable.AckIcn, allTable.AckIcnDate, 
				allTable.AckIcnTime, allTable.DeliveredTSCount,  allTable.AcceptedTSCount, allTable.ErrorCode1, 
				allTable.ErrorCode2, allTable.ErrorCode3, allTable.ErrorCode4, allTable.ErrorCode5, allTable.[TimeCreated] 
			from bts_GeneratedFunctionalAcksOfInterchange(@InterchangeStatusActivityID) allTable 
			left outer join bts_SentFunctionalAcksOfInterchange(@InterchangeStatusActivityID) sentTable
				ON (allTable.ReceiverID = sentTable.ReceiverID AND
					allTable.SenderID = sentTable.SenderID AND 
					allTable.ReceiverQ = sentTable.ReceiverQ AND
					allTable.SenderQ = sentTable.SenderQ AND 
					allTable.GroupControlNo = sentTable.GroupControlNo)
			where sentTable.ActivityID is null
		)
		order by [TimeCreated]
	end
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetFunctionalAcksOfAnInterchange] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[bts_GetFunctionalAcksOfAnInterchange] TO [BTS_OPERATORS]
    AS [dbo];

