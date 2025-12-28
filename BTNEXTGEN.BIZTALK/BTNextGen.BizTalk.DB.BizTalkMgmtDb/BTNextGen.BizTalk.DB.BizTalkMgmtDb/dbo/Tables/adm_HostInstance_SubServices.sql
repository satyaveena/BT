CREATE TABLE [dbo].[adm_HostInstance_SubServices] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (256)   NOT NULL,
    [MonikerName]  NVARCHAR (256)   NOT NULL,
    [ContextParam] NVARCHAR (256)   NOT NULL,
    [Type]         INT              NOT NULL,
    [UniqueId]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL
);

