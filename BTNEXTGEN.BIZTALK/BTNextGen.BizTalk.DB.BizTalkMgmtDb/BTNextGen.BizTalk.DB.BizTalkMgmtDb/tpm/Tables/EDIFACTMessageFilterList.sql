CREATE TABLE [tpm].[EDIFACTMessageFilterList] (
    [FilterListId] INT          IDENTITY (1, 1) NOT NULL,
    [SettingsId]   INT          NOT NULL,
    [MessageId]    NVARCHAR (8) NOT NULL,
    [version]      ROWVERSION   NOT NULL,
    CONSTRAINT [PK_EDIFACTMessageFilterList] PRIMARY KEY CLUSTERED ([FilterListId] ASC),
    CONSTRAINT [FK_EDIFACTMessageFilterList_ProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[EDIFACTProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_EDIFACTMessageFilterList] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC)
);

