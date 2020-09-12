USE master
GO
CREATE DATABASE [MarketListDB]
GO

USE [MarketListDB] 
GO
/****** Object:  Table [dbo].[Markets]    Script Date: 9/11/2020 8:33:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Markets](
	[MarketListID] [int] IDENTITY(1,1) NOT NULL,
	[MarketName] [nvarchar](300) NOT NULL,
	[MarketLocation] [nvarchar](100) NOT NULL,
	[MarketSizeInHectares] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Markets] PRIMARY KEY CLUSTERED 
(
	[MarketListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Markets] ON 
GO
INSERT [dbo].[Markets] ([MarketListID], [MarketName], [MarketLocation], [MarketSizeInHectares]) VALUES (1, N'Ibadan, Oyo State, Nigeria', N'Bodija Market', CAST(500.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Markets] ([MarketListID], [MarketName], [MarketLocation], [MarketSizeInHectares]) VALUES (3, N'By Bata Bus Stop Kano State', N'Sabon Gari Market', CAST(3.50 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Markets] OFF
GO
/****** Object:  StoredProcedure [dbo].[MuyikInsertNewMarket]    Script Date: 9/11/2020 8:33:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MuyikInsertNewMarket]
@MarketName nvarchar(300),
@MarketLocation nvarchar(100),
@MarketSizeInHectares decimal(18,2)

AS

INSERT INTO Markets
                  (MarketName, MarketLocation, MarketSizeInHectares)
VALUES (@MarketName,@MarketLocation,@MarketSizeInHectares)
GO
/****** Object:  StoredProcedure [dbo].[SelectAllMarkets]    Script Date: 9/11/2020 8:33:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----Stored Procedure To Fetch All Markets
CREATE PROCEDURE [dbo].[SelectAllMarkets]
AS
SELECT MarketListID, MarketName, MarketLocation, MarketSizeInHectares
FROM     Markets
GO
