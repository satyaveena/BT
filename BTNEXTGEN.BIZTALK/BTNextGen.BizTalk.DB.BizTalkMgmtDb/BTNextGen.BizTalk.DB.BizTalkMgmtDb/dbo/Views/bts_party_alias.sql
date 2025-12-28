CREATE VIEW [dbo].[bts_party_alias] 
WITH SCHEMABINDING AS 
SELECT DISTINCT TOP (100) PERCENT MIN(Idntty.Id) AS nID, Prtnr.PartnerId AS nPartyID, MAX(Idntty.Description) AS nvcName, 
Idntty.Qualifier AS nvcQualifier, Idntty.Value AS nvcValue, MAX(Idntty.DateModified) as DateModified 
FROM tpm.BusinessIdentity AS Idntty INNER JOIN 
tpm.BusinessProfile AS Prfl ON Idntty.ProfileId = Prfl.ProfileId INNER JOIN 
tpm.Partner AS Prtnr ON Prfl.PartnerId = Prtnr.PartnerId 
GROUP BY Idntty.Qualifier, Idntty.Value, Prtnr.PartnerId 
Union 
Select 0 as nID, Prtnr.PartnerId, 'Organization' as nvcName, 'OrganizationName' as nvcQualifier, 
Prtnr.Name as nvcValue, Prtnr.DateModified 
From tpm.Partner Prtnr 
GO
CREATE TRIGGER [dbo].[Trig_INS_bts_party_alias]
   ON  [dbo].[bts_party_alias]
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	declare @ProfileId int
	set @ProfileId = 0
	begin transaction
	if not exists(Select Prfl.ProfileId
					from tpm.BusinessProfile Prfl with (UPDLOCK, HOLDLOCK),
						tpm.Partner Prtnr, inserted I
					where Prfl.PartnerId = Prtnr.PartnerId
						and Prtnr.PartnerId = I.nPartyID)
	Begin
		declare @PartnerId int
		Select @PartnerId = nPartyID from inserted
		
		Insert into tpm.BusinessProfile (PartnerId, Name)
			values (@PartnerId, 'Default')
	End
	commit
	Insert into tpm.BusinessIdentity (ProfileId, Name, Description, Qualifier, Value, DateModified)
		Select Prfl.ProfileId, 'QualifierIdentity', I.nvcName, I.nvcQualifier, I.nvcValue, I.DateModified
	From tpm.BusinessProfile Prfl, Inserted I
	Where Prfl.PartnerId = I.nPartyID
END

GO
CREATE TRIGGER [dbo].[Trig_UPD_bts_party_alias]
   ON  [dbo].[bts_party_alias]
   INSTEAD OF Update 
AS 
BEGIN
	SET NOCOUNT ON;
	declare @oldQualifier nvarchar(64)
	declare @oldValue nvarchar(256)
	Select @oldQualifier = Idntty.Qualifier, @oldValue = Idntty.Value
	From tpm.BusinessIdentity Idntty, inserted I
	Where Idntty.Id = I.nID
	
	Update tpm.BusinessIdentity Set Description = i.nvcName, Qualifier = i.nvcQualifier, Value = i.nvcValue, DateModified = i.DateModified
	FROM    inserted i
	Where Qualifier = @oldQualifier and Value = @oldValue
END

GO
CREATE TRIGGER [dbo].[Trig_DEL_bts_party_alias]
   ON  [dbo].[bts_party_alias]
   INSTEAD OF Delete 
AS 
BEGIN
	SET NOCOUNT ON;
	declare @oldQualifier nvarchar(64)
	declare @oldValue nvarchar(256)
	Select @oldQualifier = Idntty.Qualifier, @oldValue = Idntty.Value
	From tpm.BusinessIdentity Idntty, deleted I
	Where Idntty.Id = I.nID
	
	Delete From tpm.BusinessIdentity
	Where Qualifier = @oldQualifier and Value = @oldValue
END

GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_alias] TO [BTS_OPERATORS]
    AS [dbo];

