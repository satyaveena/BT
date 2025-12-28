CREATE TABLE [dbo].[adm_ServiceClass] (
    [Id]                        INT              IDENTITY (1, 1) NOT NULL,
    [Name]                      NVARCHAR (256)   NOT NULL,
    [UniqueId]                  UNIQUEIDENTIFIER NOT NULL,
    [LowWatermark]              INT              NOT NULL,
    [HighWatermark]             INT              NOT NULL,
    [BatchSize]                 INT              NOT NULL,
    [SingleDequeueSession]      INT              NOT NULL,
    [SerializeInstanceDelivery] INT              NOT NULL,
    [GroupBatchByInstance]      INT              NOT NULL,
    [LowMemorymark]             INT              NOT NULL,
    [HighMemorymark]            INT              NOT NULL,
    [ThrottleFlag]              INT              NOT NULL,
    [LowSessionmark]            INT              NOT NULL,
    [HighSessionmark]           INT              NOT NULL,
    [CacheInstanceState]        INT              NOT NULL,
    [MaxDequeueThread]          INT              NOT NULL,
    CONSTRAINT [adm_ServiceClass_unique_key] UNIQUE NONCLUSTERED ([Name] ASC)
);

