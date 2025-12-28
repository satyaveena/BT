CREATE procedure [dbo].[admsvr_PartnerPortToPEP]
@bstrPartyValue nvarchar(256),
@bstrPartyQualifier nvarchar(64),
@bstrOperationName nvarchar(256),
@service_port uniqueidentifier,
@bstrPartyName nvarchar(256) OUTPUT			-- CHudnall - EBiz Suite PS Bug #19332
AS
set nocount on
declare @nPartyID int,
        @nRolePortTypeID int,
        @nPortTypeID int,
		@nRoleID int,
		@nOperationID int,
		@nSendPortID int
SELECT	@nPartyID = pa.nPartyID, 
		@bstrPartyName = p.nvcName 
FROM 	bts_party_alias pa
INNER JOIN bts_party p ON p.nID = pa.nPartyID
WHERE 	pa.nvcValue = @bstrPartyValue AND
     	pa.nvcQualifier = @bstrPartyQualifier
SELECT @nRolePortTypeID = bsp.nRolePortTypeID 
FROM bts_orchestration_port AS bsp
WHERE bsp.uidGUID = @service_port AND 
	bsp.nRolePortTypeID is not null
SELECT @nRoleID = brp.nRoleID, @nPortTypeID = brp.nPortTypeID 
FROM  bts_role_porttype AS brp
WHERE  brp.nID = @nRolePortTypeID
SELECT @nOperationID = bpo.nID
FROM bts_porttype_operation AS bpo
WHERE bpo.nvcName = @bstrOperationName AND
     bpo.nPortTypeID = @nPortTypeID
SELECT  @nSendPortID = pse.nSendPortID
FROM bts_enlistedparty_operation_mapping AS beom,
     bts_enlistedparty_port_mapping as bepm,
     bts_enlistedparty AS be,
     bts_party_sendport as pse
WHERE be.nRoleID = @nRoleID AND
      be.nPartyID = @nPartyID  AND
      be.nID = bepm.nEnlistedPartyID AND
      bepm.nRolePortTypeID = @nRolePortTypeID AND
      bepm.nID = beom.nPortMappingID AND
      beom.nOperationID =@nOperationID AND
      pse.nID = beom.nPartySendPortID
SELECT  spt.uidGUID,
        spt.bIsPrimary
        
    FROM bts_sendport sp
    INNER JOIN bts_sendport_transport spt ON spt.nSendPortID = sp.nID
    WHERE   sp.nID = @nSendPortID
    ORDER BY sp.nID, spt.bIsPrimary DESC
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_PartnerPortToPEP] TO [BTS_HOST_USERS]
    AS [dbo];

