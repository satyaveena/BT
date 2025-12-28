CREATE TABLE [dbo].[bas_Properties] (
    [PropertyName]  NVARCHAR (80)  NOT NULL,
    [PropertyValue] NVARCHAR (260) NOT NULL,
    CONSTRAINT [PK_bas_Properties] PRIMARY KEY CLUSTERED ([PropertyName] ASC)
);

