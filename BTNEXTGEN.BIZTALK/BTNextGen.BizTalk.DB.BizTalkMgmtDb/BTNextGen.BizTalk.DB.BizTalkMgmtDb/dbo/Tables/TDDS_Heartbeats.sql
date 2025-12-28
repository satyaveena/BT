CREATE TABLE [dbo].[TDDS_Heartbeats] (
    [ServiceID]        UNIQUEIDENTIFIER NULL,
    [SourceID]         UNIQUEIDENTIFIER NULL,
    [Age]              INT              IDENTITY (1, 1) NOT NULL,
    [TimeLastChanged]  DATETIME         NULL,
    [RecordsProcessed] INT              NULL,
    [Latency]          FLOAT (53)       NULL,
    [EventsProcessed]  INT              NULL,
    [RecordsLeft]      INT              NULL,
    [ErrorCode]        INT              NULL,
    [ErrorDescription] NVARCHAR (1024)  NULL,
    FOREIGN KEY ([ServiceID]) REFERENCES [dbo].[TDDS_Services] ([ServiceID]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [TDDS_HeartBeatClusteredIndex]
    ON [dbo].[TDDS_Heartbeats]([ServiceID] ASC, [SourceID] ASC);


GO
CREATE NONCLUSTERED INDEX [TDDS_HeartBeatNonClusteredIndex]
    ON [dbo].[TDDS_Heartbeats]([Age] ASC);

