CREATE PROCEDURE [dbo].[TDDS_UpsertTDDS_StreamStatus]
    @serverName     nvarchar(128),
    @destinationId  tinyint,
    @partitionId    tinyint,
    @lastSeqNum		bigint,
    @eventIdx       int = NULL
    
 AS
    
    UPDATE TDDS_StreamStatus
    SET lastSeqNum = @lastSeqNum,
        eventIdx = @eventIdx
    WHERE serverName=@serverName and destinationId = @destinationId and partitionId = @partitionId
    
    IF (@@ROWCOUNT = 0)
    BEGIN
        INSERT TDDS_StreamStatus(serverName, destinationId, partitionId, lastSeqNum, eventIdx) 
        VALUES ( @serverName, @destinationId, @partitionId, @lastSeqNum, @eventIdx )
    END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TDDS_UpsertTDDS_StreamStatus] TO [BAM_EVENT_WRITER]
    AS [dbo];

