CREATE VIEW [dbo].[bts_party_sendport]
AS
SELECT     tpm.SendPortReference.Id AS nID, tpm.SendPortReference.PartnerId AS nPartyID, dbo.bts_sendport.nID AS nSendPortID,
				tpm.SendPortReference.SequenceNumber AS nSequence, tpm.SendPortReference.DateModified
FROM         tpm.SendPortReference INNER JOIN
                      dbo.bts_sendport ON tpm.SendPortReference.Name = dbo.bts_sendport.nvcName

GO
CREATE TRIGGER [dbo].[Trig_INS_bts_party_sendport]
   ON  [dbo].[bts_party_sendport]
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	Insert into tpm.SendPortReference (PartnerId, Name, SequenceNumber, DateModified)
		Select I.nPartyID, SndPrt.nvcName, I.nSequence, I.DateModified
		From bts_sendport SndPrt, inserted I
		Where I.nSendPortID = SndPrt.nID
END

GO
CREATE TRIGGER [dbo].[Trig_UPD_bts_party_sendport]
   ON  [dbo].[bts_party_sendport]
   INSTEAD OF Update 
AS 
BEGIN
	SET NOCOUNT ON;
	Update tpm.SendPortReference set PartnerId = I.nPartyID, Name = SndPrt.nvcName, SequenceNumber = I.nSequence, DateModified = I.DateModified
	From inserted I, bts_sendport SndPrt
	Where tpm.SendPortReference.Id = I.nID and SndPrt.nID = I.nSendPortID
	
END

GO
CREATE TRIGGER [dbo].[Trig_DEL_bts_party_sendport]
   ON  [dbo].[bts_party_sendport]
   INSTEAD OF Delete 
AS 
BEGIN
	SET NOCOUNT ON;
	declare @deletedId int
	Select @deletedId = nID from deleted
	
	Delete From tpm.SendPortReference
	Where Id = @deletedId
END

GO
GRANT DELETE
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_ADMIN_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_HOST_USERS]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[bts_party_sendport] TO [BTS_OPERATORS]
    AS [dbo];

