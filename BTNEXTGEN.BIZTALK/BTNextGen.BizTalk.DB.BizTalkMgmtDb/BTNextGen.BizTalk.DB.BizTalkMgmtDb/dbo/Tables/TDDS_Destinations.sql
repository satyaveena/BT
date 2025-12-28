CREATE TABLE [dbo].[TDDS_Destinations] (
    [DestinationID]    UNIQUEIDENTIFIER NOT NULL,
    [DestinationName]  NVARCHAR (256)   NULL,
    [ConnectionString] NVARCHAR (1024)  NOT NULL,
    PRIMARY KEY CLUSTERED ([DestinationID] ASC)
);

