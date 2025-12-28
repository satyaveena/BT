CREATE TABLE [dbo].[adm_Host] (
    [Id]                               INT            IDENTITY (1, 1) NOT NULL,
    [GroupId]                          INT            NOT NULL,
    [Name]                             NVARCHAR (80)  NOT NULL,
    [NTGroupName]                      NVARCHAR (128) NOT NULL,
    [DateModified]                     DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [LastUsedLogon]                    NVARCHAR (128) NOT NULL,
    [HostTracking]                     INT            NOT NULL,
    [AuthTrusted]                      INT            NOT NULL,
    [HostType]                         INT            NOT NULL,
    [DecryptCertName]                  NVARCHAR (256) NOT NULL,
    [DecryptCertThumbprint]            NVARCHAR (80)  NOT NULL,
    [ClusterResourceGroupName]         NVARCHAR (256) NOT NULL,
    [IsHost32BitOnly]                  BIT            NOT NULL,
    [MessageDeliverySampleSpaceSize]   INT            DEFAULT ((100)) NOT NULL,
    [MessageDeliverySampleSpaceWindow] INT            DEFAULT ((15000)) NOT NULL,
    [MessageDeliveryOverdriveFactor]   INT            DEFAULT ((125)) NOT NULL,
    [MessageDeliveryMaximumDelay]      INT            DEFAULT ((300000)) NOT NULL,
    [MessagePublishSampleSpaceSize]    INT            DEFAULT ((100)) NOT NULL,
    [MessagePublishSampleSpaceWindow]  INT            DEFAULT ((15000)) NOT NULL,
    [MessagePublishOverdriveFactor]    INT            DEFAULT ((125)) NOT NULL,
    [MessagePublishMaximumDelay]       INT            DEFAULT ((300000)) NOT NULL,
    [DeliveryQueueSize]                INT            DEFAULT ((100)) NOT NULL,
    [DBSessionThreshold]               INT            DEFAULT ((0)) NOT NULL,
    [GlobalMemoryThreshold]            INT            DEFAULT ((0)) NOT NULL,
    [ProcessMemoryThreshold]           INT            DEFAULT ((25)) NOT NULL,
    [ThreadThreshold]                  INT            DEFAULT ((0)) NOT NULL,
    [DBQueueSizeThreshold]             INT            DEFAULT ((50000)) NOT NULL,
    [InflightMessageThreshold]         INT            DEFAULT ((1000)) NOT NULL,
    [ThreadPoolSize]                   INT            DEFAULT ((20)) NOT NULL,
    CONSTRAINT [adm_Host_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_Host_DBQueueSizeThreshold] CHECK ([DBQueueSizeThreshold]>=(0)),
    CONSTRAINT [adm_Host_DBSessionThreshold] CHECK ([DBSessionThreshold]>=(0)),
    CONSTRAINT [adm_Host_DeliveryQueueSize] CHECK ([DeliveryQueueSize]>=(1)),
    CONSTRAINT [adm_Host_GlobalMemoryThreshold] CHECK ([GlobalMemoryThreshold]>=(0)),
    CONSTRAINT [adm_Host_InflightMessageThreshold] CHECK ([InflightMessageThreshold]>=(0)),
    CONSTRAINT [adm_Host_MessageDeliveryMaximumDelay] CHECK ([MessageDeliveryMaximumDelay]>=(0)),
    CONSTRAINT [adm_Host_MessageDeliveryOverdriveFactor] CHECK ([MessageDeliveryOverdriveFactor]>=(100)),
    CONSTRAINT [adm_Host_MessageDeliverySampleSpaceSize] CHECK ([MessageDeliverySampleSpaceSize]>=(0)),
    CONSTRAINT [adm_Host_MessageDeliverySampleSpaceWindow] CHECK ([MessageDeliverySampleSpaceWindow]>=(0)),
    CONSTRAINT [adm_Host_MessagePublishMaximumDelay] CHECK ([MessagePublishMaximumDelay]>=(0)),
    CONSTRAINT [adm_Host_MessagePublishOverdriveFactor] CHECK ([MessagePublishOverdriveFactor]>=(100)),
    CONSTRAINT [adm_Host_MessagePublishSampleSpaceSize] CHECK ([MessagePublishSampleSpaceSize]>=(0)),
    CONSTRAINT [adm_Host_MessagePublishSampleSpaceWindow] CHECK ([MessagePublishSampleSpaceWindow]>=(0)),
    CONSTRAINT [adm_Host_ProcessMemoryThreshold] CHECK ([ProcessMemoryThreshold]>=(0)),
    CONSTRAINT [adm_Host_ThreadPoolSize] CHECK ([ThreadPoolSize]>=(1) AND [ThreadPoolSize]<=(50)),
    CONSTRAINT [adm_Host_ThreadThreshold] CHECK ([ThreadThreshold]>=(0)),
    CONSTRAINT [adm_Host_fk_group] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[adm_Group] ([Id]),
    CONSTRAINT [adm_Host_unique_key] UNIQUE NONCLUSTERED ([GroupId] ASC, [Name] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Host] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Host] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Host] TO [BTS_OPERATORS]
    AS [dbo];

