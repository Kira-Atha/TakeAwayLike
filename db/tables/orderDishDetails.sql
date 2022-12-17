USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[OrderDishDetails]    Script Date: 27-05-21 10:41:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderDishDetails](
	[DishId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_OrderDishDetails] PRIMARY KEY CLUSTERED 
(
	[DishId] ASC,
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderDishDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDishDetails_Dishes] FOREIGN KEY([DishId])
REFERENCES [dbo].[Dishes] ([DishId])
GO

ALTER TABLE [dbo].[OrderDishDetails] CHECK CONSTRAINT [FK_OrderDishDetails_Dishes]
GO

ALTER TABLE [dbo].[OrderDishDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDishDetails_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO

ALTER TABLE [dbo].[OrderDishDetails] CHECK CONSTRAINT [FK_OrderDishDetails_Orders]
GO

