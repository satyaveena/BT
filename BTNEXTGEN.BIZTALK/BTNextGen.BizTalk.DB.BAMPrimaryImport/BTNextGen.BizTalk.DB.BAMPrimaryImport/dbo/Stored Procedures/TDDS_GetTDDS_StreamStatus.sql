CREATE PROCEDURE [dbo].[TDDS_GetTDDS_StreamStatus]
@serverName		nvarchar(128),
@destinationId	tinyint,
@partitionId	tinyint
 AS
SELECT lastSeqNum, eventIdx
FROM TDDS_StreamStatus
WHERE serverName = @serverName AND destinationId = @destinationId AND partitionId = @partitionId
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_GetTDDS_StreamStatus] TO [BAM_EVENT_WRITER]
    AS [dbo];

