CREATE TABLE [tpm].[EDIFACTValidationOverrides] (
    [OverridesId]                            INT           IDENTITY (1, 1) NOT NULL,
    [SettingsId]                             INT           NOT NULL,
    [MessageId]                              NVARCHAR (15) NOT NULL,
    [EnforceCharacterSet]                    BIT           NOT NULL,
    [ValidateEDITypes]                       BIT           NOT NULL,
    [ValidateExtended]                       BIT           NOT NULL,
    [AllowLeadingAndTrailingSpacesAndZeroes] BIT           NOT NULL,
    [TrailingSeparatorPolicy]                SMALLINT      NOT NULL,
    [version]                                ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EDIFACTValidationOverrides] PRIMARY KEY CLUSTERED ([OverridesId] ASC),
    CONSTRAINT [FK_EDIFACTValidationOverrides_EDIFACTProtocolSettings] FOREIGN KEY ([SettingsId]) REFERENCES [tpm].[EDIFACTProtocolSettings] ([SettingsId]) ON DELETE CASCADE,
    CONSTRAINT [UK_EDIFACTValidationOverrides] UNIQUE NONCLUSTERED ([SettingsId] ASC, [MessageId] ASC)
);

