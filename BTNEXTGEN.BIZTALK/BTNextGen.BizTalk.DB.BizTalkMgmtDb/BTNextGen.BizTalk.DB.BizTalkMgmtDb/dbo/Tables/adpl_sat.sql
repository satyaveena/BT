CREATE TABLE [dbo].[adpl_sat] (
    [id]            INT            IDENTITY (1, 1) NOT NULL,
    [applicationId] INT            NOT NULL,
    [sdmType]       NVARCHAR (256) NULL,
    [luid]          NVARCHAR (440) NULL,
    [properties]    NTEXT          NULL,
    [files]         NTEXT          NULL,
    [cabContent]    IMAGE          NULL,
    CONSTRAINT [PK_adpl_sat] PRIMARY KEY NONCLUSTERED ([id] ASC),
    CONSTRAINT [FK_bts_application_adpl_sat] FOREIGN KEY ([applicationId]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [UQ_adpl_sat] UNIQUE CLUSTERED ([luid] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_adpl_sat]
    ON [dbo].[adpl_sat]([applicationId] ASC);


GO
GRANT SELECT
    ON OBJECT::[dbo].[adpl_sat] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adpl_sat] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[adpl_sat] TO [BTS_OPERATORS]
    AS [dbo];

