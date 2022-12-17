USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[Restaurants]    Script Date: 27-05-21 10:42:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Restaurants](
	[Name] [nvarchar](100) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[Adress] [nvarchar](100) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[RestaurantType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[TvaNumber] [nvarchar](50) NOT NULL,
	[RestorerId] [int] NOT NULL,
	[Country] [nvarchar](100) NOT NULL,
	[RestaurantId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Restaurants_1] PRIMARY KEY CLUSTERED 
(
	[RestaurantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Restaurants]  WITH CHECK ADD  CONSTRAINT [FK_Restaurants_Restorers] FOREIGN KEY([RestorerId])
REFERENCES [dbo].[Restorers] ([RestorerId])
GO

ALTER TABLE [dbo].[Restaurants] CHECK CONSTRAINT [FK_Restaurants_Restorers]
GO

