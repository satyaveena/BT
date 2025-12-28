CREATE TABLE [dbo].[bam_TrackingProfiles] (
    [nvcName]         NVARCHAR (128)   NOT NULL,
    [uidVersionId]    UNIQUEIDENTIFIER NOT NULL,
    [nMinorVersionId] INT              NOT NULL,
    [nID]             INT              IDENTITY (1, 1) NOT NULL
);

