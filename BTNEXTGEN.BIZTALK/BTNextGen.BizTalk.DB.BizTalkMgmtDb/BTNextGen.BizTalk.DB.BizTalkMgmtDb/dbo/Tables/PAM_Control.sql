CREATE TABLE [dbo].[PAM_Control] (
    [EdiMessageType]    SMALLINT       NOT NULL,
    [ActionType]        NVARCHAR (50)  NOT NULL,
    [ActionDateTime]    DATETIME       NULL,
    [UsedOnce]          BIT            NOT NULL,
    [BatchId]           BIGINT         NOT NULL,
    [BatchName]         NVARCHAR (256) NOT NULL,
    [SenderPartyName]   NVARCHAR (256) NOT NULL,
    [ReceiverPartyName] NVARCHAR (256) NOT NULL,
    [AgreementName]     NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_PAM_Control] PRIMARY KEY CLUSTERED ([UsedOnce] ASC, [BatchId] ASC),
    CONSTRAINT [FK_PAM_Control] FOREIGN KEY ([BatchId]) REFERENCES [tpm].[BatchDescription] ([Id]) ON DELETE CASCADE
);

