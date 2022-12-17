USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[Dishes]    Script Date: 27-05-21 10:41:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Dishes](
	[DishId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[TypeService] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TypeDish] [nvarchar](50) NOT NULL,
	[RestaurantId] [int] NOT NULL,
 CONSTRAINT [PK_Dishes] PRIMARY KEY CLUSTERED 
(
	[DishId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Dishes]  WITH CHECK ADD  CONSTRAINT [FK_Dishes_Restaurants] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([RestaurantId])
GO

ALTER TABLE [dbo].[Dishes] CHECK CONSTRAINT [FK_Dishes_Restaurants]
GO

