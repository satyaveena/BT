CREATE TABLE [dbo].[TDDS_Sources] (
    [SourceID]          UNIQUEIDENTIFIER NOT NULL,
    [DestinationID]     UNIQUEIDENTIFIER NULL,
    [SourceName]        NVARCHAR (256)   NULL,
    [ConnectionString]  NVARCHAR (1024)  NOT NULL,
    [StreamType]        INT              NULL,
    [AcceptableLatency] INT              NULL,
    [Enabled]           BIT              DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([SourceID] ASC),
    FOREIGN KEY ([DestinationID]) REFERENCES [dbo].[TDDS_Destinations] ([DestinationID])
);

