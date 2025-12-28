CREATE TABLE [dbo].[bts_receiveport] (
    [nID]                 INT              IDENTITY (1, 1) NOT NULL,
    [nvcName]             NVARCHAR (256)   NOT NULL,
    [bTwoWay]             BIT              NOT NULL,
    [nAuthentication]     INT              NOT NULL,
    [nSendPipelineId]     INT              NULL,
    [nvcSendPipelineData] NTEXT            NULL,
    [nTracking]           INT              NULL,
    [uidGUID]             UNIQUEIDENTIFIER NOT NULL,
    [nvcCustomData]       NTEXT            NULL,
    [DateModified]        DATETIME         NOT NULL,
    [nApplicationID]      INT              NOT NULL,
    [nvcDescription]      NVARCHAR (1024)  NULL,
    [bRouteFailedMessage] BIT              DEFAULT ((0)) NULL,
    CONSTRAINT [bts_receiveport_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_receiveport_foreign_applicationid] FOREIGN KEY ([nApplicationID]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [bts_receiveport_foreign_sendpipelineid] FOREIGN KEY ([nSendPipelineId]) REFERENCES [dbo].[bts_pipeline] ([Id]),
    CONSTRAINT [bts_receiveport_unique_name] UNIQUE NONCLUSTERED ([nvcName] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_receiveport] TO [BTS_OPERATORS]
    AS [dbo];

