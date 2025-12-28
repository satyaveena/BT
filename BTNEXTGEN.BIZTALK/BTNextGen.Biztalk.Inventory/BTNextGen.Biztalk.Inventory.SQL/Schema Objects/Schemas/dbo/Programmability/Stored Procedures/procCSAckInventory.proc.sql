USE [ProductCatalog]
GO

/****** Object:  StoredProcedure [dbo].[procCSAckInventory]    Script Date: 05/24/2011 07:27:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[procCSAckInventory] 
(
	@CSAckInventory  [dbo].[udtCSAckInventory] READONLY
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE		dbo.Inventory
	SET			[LoadStatus] = 'Failed'
	FROM		dbo.Inventory P (NOLOCK)
	INNER JOIN	@CSAckInventory C ON P.BTKEY = C.BTKey
	WHERE		P.LoadStatus = 'Loaded' 

END


GO

