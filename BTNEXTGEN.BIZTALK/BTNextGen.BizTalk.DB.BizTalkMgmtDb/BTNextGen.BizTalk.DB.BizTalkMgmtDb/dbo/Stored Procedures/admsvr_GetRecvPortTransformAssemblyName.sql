create procedure [dbo].[admsvr_GetRecvPortTransformAssemblyName]
@RecvPortId uniqueidentifier,
@InMessageType nvarchar(256),
@IsTransmit bit
AS
set nocount on
declare @nMapID uniqueidentifier
SELECT @nMapID = ms.id
FROM bt_MapSpec ms
	INNER JOIN bt_DocumentSpec ds ON ds.docspec_name = ms.indoc_docspec_name
	INNER JOIN bts_receiveport_transform rpt ON rpt.uidTransformGUID = ms.id 
	INNER JOIN bts_receiveport rp ON rp.nID = rpt.nReceivePortID
WHERE	ds.msgtype = @InMessageType AND
		rp.uidGUID = @RecvPortId	AND
		rpt.bTransmit = @IsTransmit
SELECT 	ba.nvcFullName,
		bi.FullName
FROM bts_assembly ba, bts_item bi, bt_MapSpec ms 
	
WHERE	@nMapID = ms.id AND
		ba.nID = ms.assemblyid AND
		bi.id = ms.itemid
	
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetRecvPortTransformAssemblyName] TO [BTS_HOST_USERS]
    AS [dbo];

