CREATE TABLE [dbo].[bts_dynamicport_subids] (
    [uidSendPortID]  UNIQUEIDENTIFIER NOT NULL,
    [nSendHandlerID] INT              NOT NULL,
    [uidGUID]        UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [nvcHostName]    NVARCHAR (80)    NULL
);

