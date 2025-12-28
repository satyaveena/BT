USE [ProductCatalog]
GO

/****** Object:  StoredProcedure [dbo].[procCSGetInventory]    Script Date: 05/24/2011 07:23:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Balaji
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[procCSGetInventory] 
	-- Add the parameters for the stored procedure here
	@CatalogName	VARCHAR(50),
	@MaxProducts	INT		
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE	@FromBTKey		CHAR(10)
	DECLARE	@ToBTKey		CHAR(10)

	DECLARE @RC int
	DECLARE @ProductCatalog varchar(50)
	DECLARE @MinBTKey char(10)
	DECLARE @MaxBTKey char(10)
	DECLARE @ReturnCode bit
	DECLARE @ReturnString varchar(2000)
	DECLARE @InventoryCatalog varchar(50)
	
	SET	@InventoryCatalog = @CatalogName;

	EXECUTE @RC = [ProductCatalog].[dbo].[procGetInventoryCatalogBTKeyRange] 
	   @InventoryCatalog
	  ,@MinBTKey OUTPUT
	  ,@MaxBTKey OUTPUT
	  ,@ReturnString OUTPUT

	IF @RC = 1
		RETURN 1;
	if @RC = 0 
	BEGIN	
	DECLARE @CSInventory TABLE (InventoryID INT, CSLoadStatus VARCHAR(25) );
	
	UPDATE	dbo.Inventory
	SET		[LoadStatus] = 'Loaded'
	OUTPUT	INSERTED.InventoryID,
			DELETED.[LoadStatus]			
	INTO	@CSInventory
	WHERE	InventoryID IN
			(
				SELECT	TOP (@MaxProducts) InventoryID
				FROM	dbo.Inventory P (NOLOCK)
				WHERE	
						P.[LoadStatus] IN ('I', 'U', 'Inserted', 'Updated') 
				AND		BTKEY BETWEEN @MinBTKey AND @MaxBTKey)

			
		
		
	SELECT 
	P.[BTKEY]												  AS [BTKEY] ,
	[dbo].[udfCSGetVirtualInventory] (P.BTKEY,P.WarehouseCode) AS [VirtualInventory],
	P.[GTIN]												  AS [GTIN],
	[dbo].[udfCSWarehouseLookup] (P.WarehouseCode)	AS [WarehouseCode],
	P.InventoryType											  AS [InventoryType],
	P.AvailableQuantity										  AS [AvailableQuantity],
	P.OnOrderQuantity										  AS [OnOrderQuantity] ,
	P.LEAvailableQuantity									  AS [LEAvailableQuantity]
	FROM	
			[dbo].[Inventory] P
			INNER JOIN @CSInventory C ON P.InventoryID = C.InventoryID	
	WHERE 
			P.InventoryType in ('A','')	
			
	SELECT @ProductCatalog = [Literal] FROM [ProductCatalog].[dbo].[CommerceServerProductCatalog]
	WHERE [StartBTKey] = @MinBTKey AND [EndBTKey] = @MaxBTKey 		
			
	SELECT @@ROWCOUNT AS [ProductCount] , @CatalogName as [InventoryCatalogName] , @ProductCatalog as [ProductCatalogName]
	RETURN 0;
	END			
	
END







GO


