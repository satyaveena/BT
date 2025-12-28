CREATE TABLE [dbo].[bts_rolelink] (
    [nID]              INT            IDENTITY (1, 1) NOT NULL,
    [nvcName]          NVARCHAR (256) NOT NULL,
    [nvcFullName]      NVARCHAR (256) NULL,
    [nOrchestrationID] INT            NOT NULL,
    [nRoleID]          INT            NOT NULL,
    [bImplements]      BIT            NOT NULL,
    [nBindingType]     INT            NOT NULL,
    CONSTRAINT [PK_bts_rolelink] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [FK_bts_rolelink_bts_orchestration] FOREIGN KEY ([nOrchestrationID]) REFERENCES [dbo].[bts_orchestration] ([nID]) ON DELETE CASCADE,
    CONSTRAINT [FK_bts_rolelink_bts_role] FOREIGN KEY ([nRoleID]) REFERENCES [dbo].[bts_role] ([nID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_rolelink] TO [BTS_OPERATORS]
    AS [dbo];

