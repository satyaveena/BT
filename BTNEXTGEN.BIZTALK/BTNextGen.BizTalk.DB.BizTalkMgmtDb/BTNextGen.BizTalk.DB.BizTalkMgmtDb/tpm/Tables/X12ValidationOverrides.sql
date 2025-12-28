CREATE TABLE [tpm].[X12ValidationOverrides] (
    [OverridesId]                            INT          IDENTITY (1, 1) NOT NULL,
    [SettingsId]                             INT          NOT NULL,
    [MessageId]                              NVARCHAR (5) NOT NULL,
    [ValidateCharacterSet]                   BIT          NOT NULL,
    [ValidateEDITypes]                       BIT          NOT NULL,
    [ValidateExtended]                       BIT          NOT NULL,
    [AllowLeadingAndTrailingSpacesAndZeroes] BIT          NOT NULL,
    [TrailingSeparatorPolicy]                SMALLINT     NOT NULL,
    [version]                                ROWVERSION   NOT NULL,
    CONSTRAINT [PK_X12ValidationOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_X12ValidationOverrides_X12ProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[X12ProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_X12ValidationOverridesKey] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC)
);

