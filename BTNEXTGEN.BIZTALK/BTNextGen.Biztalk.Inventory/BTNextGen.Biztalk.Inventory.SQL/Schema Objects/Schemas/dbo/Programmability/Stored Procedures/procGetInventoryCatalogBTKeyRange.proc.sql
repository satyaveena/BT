USE [ProductCatalog]
GO

/****** Object:  StoredProcedure [dbo].[procGetInventoryCatalogBTKeyRange]    Script Date: 05/24/2011 07:27:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[procGetInventoryCatalogBTKeyRange]
(
	@InventoryCatalog varchar(50),
	@MinBTKey		char(10)		OUTPUT,
	@MaxBTKey		char(10)		OUTPUT,
	@ReturnString	VARCHAR(2000)	OUTPUT

)
AS
BEGIN
/*
Created By:		Balaji
Created Date:	2011-05-11
Remark:			Products are stored in one of six Commerce Server Catalogs.  The algorithm for placing the product
				in a catalog is as follows
Business Logic:
                0000000000                0003000000                -        Book Inventory Catalog 1 
                0003000001                0007000000                -        Book Invenory Catalog 2 
                0007000001                0010000000                -        Book Invenotyr Catalog 3 
                0010000001                5999999999                -        Book Inventory Catalog 4 
                6000000000                 7000000000                -       Entertainment Inventory Catalog 1 
                7000000001                9999999999                -        Book Inventory Catalog 5 

INPUT:  @ProductCatalog
OUTPUT:	@MinBTKey. @MaxBTKey
*/
	SET		@ReturnString = ''

	SELECT	@MinBTKey = StartBTkey,
			@MaxBTKey = EndBTKey
	FROM	[dbo].[CommerceServerInventoryCatalog]
	WHERE	Literal = @InventoryCatalog
	
	IF @@ROWCOUNT = 0
	BEGIN
		SET		@ReturnString = 'Invalid Inventory Catalog'
		RETURN	1 --ERROR
	END

	RETURN 0 --SUCCESS

		
END





GO

