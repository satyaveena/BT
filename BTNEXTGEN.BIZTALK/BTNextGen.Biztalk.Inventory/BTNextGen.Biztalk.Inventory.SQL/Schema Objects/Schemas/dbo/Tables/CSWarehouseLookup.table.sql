USE [ProductCatalog]
GO

/****** Object:  Table [dbo].[CSWarehouseLookup]    Script Date: 05/24/2011 07:30:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CSWarehouseLookup](
	[ID] [dbo].[udtIdentity] IDENTITY(1,1) NOT NULL,
	[ERPWareHouse] [char](3) NOT NULL,
	[CSWareHouse] [nvarchar](15) NOT NULL,
	[CreatedBy] [dbo].[udtCreatedBy] NULL,
	[CreatedDateTime] [dbo].[udtCreatedDateTime] NULL,
	[UpdatedBy] [dbo].[udtUpdatedBy] NULL,
	[UpdatedDateTime] [dbo].[udtUpdatedDateTime] NULL,
 CONSTRAINT [PK_[CSWarehouseLookup_] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

