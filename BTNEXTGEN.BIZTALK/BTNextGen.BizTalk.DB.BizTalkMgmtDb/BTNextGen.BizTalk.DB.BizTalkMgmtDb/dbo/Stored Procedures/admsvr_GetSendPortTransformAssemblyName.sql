create procedure [dbo].[admsvr_GetSendPortTransformAssemblyName]
@SendPortID uniqueidentifier,
@InMessageType nvarchar(256),
@IsReceive bit
AS
set nocount on
declare @nMapID uniqueidentifier
SELECT @nMapID = ms.id
FROM bt_MapSpec ms
	INNER JOIN bt_DocumentSpec ds ON ds.docspec_name = ms.indoc_docspec_name
	INNER JOIN bts_sendport_transform spt ON spt.uidTransformGUID = ms.id 
	INNER JOIN bts_sendport sp ON sp.nID = spt.nSendPortID
WHERE	ds.msgtype = @InMessageType AND
		sp.uidGUID = @SendPortID	AND
		spt.bReceive = @IsReceive
SELECT 	ba.nvcFullName,
		bi.FullName
FROM bts_assembly ba, bts_item bi, bt_MapSpec ms 
	
WHERE	@nMapID = ms.id AND
		ba.nID = ms.assemblyid AND
		bi.id = ms.itemid
	
set nocount off

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[admsvr_GetSendPortTransformAssemblyName] TO [BTS_HOST_USERS]
    AS [dbo];

