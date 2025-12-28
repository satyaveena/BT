CREATE TABLE [dbo].[bts_sendport_transport] (
    [nID]                   INT              IDENTITY (1, 1) NOT NULL,
    [nvcAddress]            NVARCHAR (256)   NOT NULL,
    [nTransportTypeId]      INT              NULL,
    [nvcTransportTypeData]  NTEXT            NULL,
    [bOrderedDelivery]      BIT              NOT NULL,
    [nDeliveryNotification] INT              NOT NULL,
    [nRetryCount]           INT              NOT NULL,
    [nRetryInterval]        INT              NOT NULL,
    [bIsServiceWindow]      BIT              NOT NULL,
    [dtFromTime]            DATETIME         NOT NULL,
    [dtToTime]              DATETIME         NOT NULL,
    [bIsPrimary]            BIT              NOT NULL,
    [bSSOMappingExists]     BIT              NOT NULL,
    [nSendPortID]           INT              NOT NULL,
    [uidGUID]               UNIQUEIDENTIFIER NULL,
    [DateModified]          DATETIME         NOT NULL,
    [nSendHandlerID]        INT              NULL,
    CONSTRAINT [bts_sendport_transport_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [bts_sendport_transport_foreign_ownerid] FOREIGN KEY ([nSendPortID]) REFERENCES [dbo].[bts_sendport] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [bts_sendport_transport_foreign_sendhandlerid] FOREIGN KEY ([nSendHandlerID]) REFERENCES [dbo].[adm_SendHandler] ([Id]),
    CONSTRAINT [bts_sendport_transport_foreign_transporttypeid] FOREIGN KEY ([nTransportTypeId]) REFERENCES [dbo].[adm_Adapter] ([Id])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_sendport_transport] TO [BTS_OPERATORS]
    AS [dbo];

