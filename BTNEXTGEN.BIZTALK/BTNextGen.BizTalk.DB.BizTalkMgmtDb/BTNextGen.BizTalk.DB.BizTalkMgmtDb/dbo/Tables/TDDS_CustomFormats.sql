CREATE TABLE [dbo].[TDDS_CustomFormats] (
    [FormatID]     UNIQUEIDENTIFIER NOT NULL,
    [DecoderClass] NVARCHAR (256)   NOT NULL,
    [DllName]      NVARCHAR (1024)  NOT NULL,
    PRIMARY KEY CLUSTERED ([FormatID] ASC)
);

