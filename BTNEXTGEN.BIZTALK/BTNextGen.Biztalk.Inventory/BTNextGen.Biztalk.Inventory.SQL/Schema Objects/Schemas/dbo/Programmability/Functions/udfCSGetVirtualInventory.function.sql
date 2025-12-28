USE [ProductCatalog]
GO

/****** Object:  UserDefinedFunction [dbo].[udfCSGetVirtualInventory]    Script Date: 05/03/2011 05:45:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Balaji Ramadass
-- Create date: 29-Apr-2011
-- Description:	Get the Concatinated Virtual Inventory For the Specified Product 
--for particular warehouse
-- =============================================
CREATE FUNCTION [dbo].[udfCSGetVirtualInventory] 
(
	-- Add the parameters for the function here
	@BTKey nvarchar(10),
	@WareHouseCode nvarchar(3)
)
RETURNS nvarchar(1000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @VirtualCatalog nvarchar(1000)
	DECLARE @VirtualInventory int
	DECLARE @TempString as nvarchar(25)

	SET @VirtualInventory = 0
--Check if any Data Exist	
	SELECT @VirtualInventory  = COUNT(Inv.[BTKEY])  
	FROM 
		[ProductCatalog].[dbo].[Inventory] Inv
	WHERE
		[BTKEY] = @BTKey AND
		[InventoryType] NOT IN ('','A') AND
		[WarehouseCode] = @WareHouseCode

	IF	@VirtualInventory <> 0 
	BEGIN
		SET @VirtualCatalog = ''
		DECLARE curVInventory CURSOR FOR
		SELECT 
			[InventoryType]+':'+ CONVERT(varchar(20),[LEAvailableQuantity]) as [CSVirtualInventory]
		FROM 
			[ProductCatalog].[dbo].[Inventory] Inv
		WHERE
			[BTKEY] = @BTKey AND
			[InventoryType] NOT IN ('','A') AND
			[WarehouseCode] = @WareHouseCode
		
		OPEN curVInventory
		FETCH NEXT FROM curVInventory INTO @TempString
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @VirtualCatalog = @VirtualCatalog +';'+ @TempString
			FETCH NEXT FROM curVInventory INTO @TempString
		END;
			CLOSE curVInventory;
			DEALLOCATE curVInventory;
			SET @VirtualCatalog = Convert(varchar(20),@VirtualInventory) +  @VirtualCatalog
		
			
	END;
	
	RETURN @VirtualCatalog

END

GO


