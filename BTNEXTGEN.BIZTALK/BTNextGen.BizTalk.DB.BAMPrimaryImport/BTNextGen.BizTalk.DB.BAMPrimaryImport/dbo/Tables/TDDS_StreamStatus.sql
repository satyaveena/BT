CREATE TABLE [dbo].[TDDS_StreamStatus] (
    [serverName]    NVARCHAR (128) NOT NULL,
    [destinationId] TINYINT        NOT NULL,
    [partitionId]   TINYINT        NOT NULL,
    [lastSeqNum]    BIGINT         NOT NULL,
    [eventIdx]      INT            NULL,
    CONSTRAINT [streamstatus_unique_key] PRIMARY KEY CLUSTERED ([serverName] ASC, [destinationId] ASC, [partitionId] ASC)
);

