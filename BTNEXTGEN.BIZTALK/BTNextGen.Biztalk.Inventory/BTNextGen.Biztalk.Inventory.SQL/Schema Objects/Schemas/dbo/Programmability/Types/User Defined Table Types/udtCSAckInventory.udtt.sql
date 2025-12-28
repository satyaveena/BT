USE [ProductCatalog]
GO

/****** Object:  UserDefinedTableType [dbo].[udtCSAckinventory]    Script Date: 05/03/2011 06:22:05 ******/
CREATE TYPE [dbo].[udtCSAckInventory] AS TABLE(
	[BTKey] [char](10) NULL,
	[CSLoadError] [nvarchar](max) NULL
)
GO


