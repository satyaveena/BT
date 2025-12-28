CREATE TABLE [tpm].[X12MessageFilterList] (
    [FilterListId] INT          IDENTITY (1, 1) NOT NULL,
    [SettingsId]   INT          NOT NULL,
    [MessageId]    NVARCHAR (5) NOT NULL,
    [version]      ROWVERSION   NOT NULL,
    CONSTRAINT [PK_X12MessageFilterList] PRIMARY KEY CLUSTERED ([FilterListId] ASC),
    CONSTRAINT [FK_X12MessageFilterList_X12ProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[X12ProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_X12MessageFilterList] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC)
);

