CREATE TABLE [dbo].[bam_TrackPoints] (
    [nTrackPointId] INT              NOT NULL,
    [nProfileId]    INT              NOT NULL,
    [nvcMsgType]    NVARCHAR (2048)  NULL,
    [uidPortId]     UNIQUEIDENTIFIER NOT NULL,
    [nDirection]    INT              NOT NULL,
    [ntxtData]      NTEXT            NOT NULL
);

