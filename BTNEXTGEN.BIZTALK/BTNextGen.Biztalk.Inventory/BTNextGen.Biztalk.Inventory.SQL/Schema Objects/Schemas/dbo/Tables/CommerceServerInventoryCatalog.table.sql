USE [ProductCatalog]
GO

/****** Object:  Table [dbo].[CommerceServerInventoryCatalog]    Script Date: 05/24/2011 07:30:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CommerceServerInventoryCatalog](
	[ID] [dbo].[udtIdentity] IDENTITY(1,1) NOT NULL,
	[Literal] [dbo].[udtLiteral] NOT NULL,
	[StartBTKey] [char](10) NOT NULL,
	[EndBTKey] [char](10) NOT NULL,
	[CreatedBy] [dbo].[udtCreatedBy] NULL,
	[CreatedDateTime] [dbo].[udtCreatedDateTime] NULL,
	[UpdatedBy] [dbo].[udtUpdatedBy] NULL,
	[UpdatedDateTime] [dbo].[udtUpdatedDateTime] NULL,
 CONSTRAINT [PK_CommerceServerInventoryCatalog_] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A system generated key to maintain uniqueness' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CommerceServerInventoryCatalog', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the Commerce Server Product Catalog used to store product information. ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CommerceServerInventoryCatalog', @level2type=N'COLUMN',@level2name=N'Literal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The first BTKey with' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CommerceServerInventoryCatalog', @level2type=N'COLUMN',@level2name=N'StartBTKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Product`s Inventory are stored in one of six Commerce Server Inventory Catalogs.  The algorithm for placing the product
				in a catalog is as follows
Business Logic:
                0000000000                0003000000                -        Book InventoryCatalog 1 
                0003000001                0007000000                -        Book InventoryCatalog 2 
                0007000001                0010000000                -        Book InventoryCatalog 3 
                0010000001                5999999999                -        Book InventoryCatalog 4 
                6000000000                 7000000000               -        Entertainment InventoryCatalog 1 
                7000000001                9999999999                -        Book InventoryCatalog 5 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CommerceServerInventoryCatalog'
GO

