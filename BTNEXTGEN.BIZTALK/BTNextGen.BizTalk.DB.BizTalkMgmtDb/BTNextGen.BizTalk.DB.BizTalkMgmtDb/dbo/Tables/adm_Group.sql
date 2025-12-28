CREATE TABLE [dbo].[adm_Group] (
    [Id]                                INT              IDENTITY (1, 1) NOT NULL,
    [Name]                              NVARCHAR (256)   NOT NULL,
    [DateModified]                      DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [BizTalkAdminGroup]                 NVARCHAR (128)   NOT NULL,
    [TrackingDBServerName]              NVARCHAR (80)    NOT NULL,
    [TrackingDBName]                    NVARCHAR (128)   NOT NULL,
    [SubscriptionDBServerName]          NVARCHAR (80)    NOT NULL,
    [SubscriptionDBName]                NVARCHAR (128)   NOT NULL,
    [TrackAnalysisServerName]           NVARCHAR (80)    NOT NULL,
    [TrackAnalysisDBName]               NVARCHAR (128)   NOT NULL,
    [BamDBServerName]                   NVARCHAR (80)    NOT NULL,
    [BamDBName]                         NVARCHAR (128)   NOT NULL,
    [RuleEngineDBServerName]            NVARCHAR (80)    NOT NULL,
    [RuleEngineDBName]                  NVARCHAR (128)   NOT NULL,
    [SSOServerName]                     NVARCHAR (80)    NOT NULL,
    [GlobalTrackingOption]              INT              NOT NULL,
    [SignCertName]                      NVARCHAR (256)   NOT NULL,
    [SignCertThumbprint]                NVARCHAR (80)    NOT NULL,
    [ConfigurationCacheRefreshInterval] INT              NOT NULL,
    [TrackingConfiguration]             IMAGE            DEFAULT (0x13000000) NOT NULL,
    [LMSFragmentSize]                   INT              NOT NULL,
    [LMSThreshold]                      INT              NOT NULL,
    [DefaultHostId]                     INT              NULL,
    [BizTalkOperatorGroup]              NVARCHAR (128)   DEFAULT ('') NOT NULL,
    [GroupPropertyIdentifier]           UNIQUEIDENTIFIER DEFAULT ('{00000000-0000-0000-0000-000000000000}') NOT NULL,
    [BizTalkB2BOperatorGroup]           NVARCHAR (128)   DEFAULT ('') NOT NULL,
    [UUID]                              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    CONSTRAINT [adm_Group_pk] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [adm_Group_fk_DefaultHost] FOREIGN KEY ([DefaultHostId]) REFERENCES [dbo].[adm_Host] ([Id]),
    CONSTRAINT [adm_Group_unique_key] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Group] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Group] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adm_Group] TO [BTS_OPERATORS]
    AS [dbo];

