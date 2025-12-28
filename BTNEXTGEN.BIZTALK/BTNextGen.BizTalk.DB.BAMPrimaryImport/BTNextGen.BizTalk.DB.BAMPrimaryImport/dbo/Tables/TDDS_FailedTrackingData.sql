CREATE TABLE [dbo].[TDDS_FailedTrackingData] (
    [SeqNum]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [Source]        NVARCHAR (256)   NOT NULL,
    [FormatID]      UNIQUEIDENTIFIER NOT NULL,
    [dtLogTime]     DATETIME         NOT NULL,
    [DestinatioNID] TINYINT          NOT NULL,
    [ErrMessage]    NTEXT            NOT NULL,
    [DataImage]     IMAGE            NOT NULL,
    CONSTRAINT [FailedTrackingData_Unique_Key] PRIMARY KEY CLUSTERED ([SeqNum] ASC)
);

