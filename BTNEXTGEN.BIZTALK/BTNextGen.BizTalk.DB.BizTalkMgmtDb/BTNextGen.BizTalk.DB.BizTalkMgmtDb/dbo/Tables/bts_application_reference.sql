CREATE TABLE [dbo].[bts_application_reference] (
    [nID]                      INT IDENTITY (1, 1) NOT NULL,
    [nApplicationID]           INT NULL,
    [nReferencedApplicationID] INT NULL,
    CONSTRAINT [bts_applicationreference_unique_key] PRIMARY KEY CLUSTERED ([nID] ASC),
    CONSTRAINT [CK_bts_application_reference_cyclic] CHECK ([dbo].[adm_IsReferencedBy]([nReferencedApplicationID],[nApplicationID])=(0)),
    CONSTRAINT [bts_application_foreign_applicationid] FOREIGN KEY ([nApplicationID]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [bts_refapplication_foreign_applicationid] FOREIGN KEY ([nReferencedApplicationID]) REFERENCES [dbo].[bts_application] ([nID]),
    CONSTRAINT [bts_application_reference_unique] UNIQUE NONCLUSTERED ([nApplicationID] ASC, [nReferencedApplicationID] ASC)
);


GO
CREATE TRIGGER CK_application_reference_delete
ON dbo.bts_application_reference
FOR DELETE
AS
BEGIN
	declare @nApplicationID as int
	declare @nReferencedApplicationID as int
	select @nApplicationID = nApplicationID, @nReferencedApplicationID = nReferencedApplicationID
		from deleted
	if (dbo.adm_CanReferenceBeDeleted(@nApplicationID, @nReferencedApplicationID) = 0)
		raiserror ('Error: Application reference cannot be deleted. CK_application_reference_delete', 16, 10)
END

GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_application_reference] TO [BTS_OPERATORS]
    AS [dbo];

