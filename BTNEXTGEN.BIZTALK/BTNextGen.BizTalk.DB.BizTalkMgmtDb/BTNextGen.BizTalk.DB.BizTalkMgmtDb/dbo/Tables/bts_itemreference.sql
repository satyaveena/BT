CREATE TABLE [dbo].[bts_itemreference] (
    [nReferringAssemblyID] INT            NOT NULL,
    [nvcAssemblyName]      NVARCHAR (256) NOT NULL,
    [nvcVersionMajor]      NVARCHAR (12)  NOT NULL,
    [nvcVersionMinor]      NVARCHAR (12)  NOT NULL,
    [nvcVersionBuild]      NVARCHAR (12)  NOT NULL,
    [nvcVersionRevision]   NVARCHAR (12)  NOT NULL,
    [nvcItemName]          NVARCHAR (256) NOT NULL,
    [nvcCulture]           NVARCHAR (25)  NOT NULL,
    [nvcPublicKeyToken]    NVARCHAR (256) NOT NULL
);

