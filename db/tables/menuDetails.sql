USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[MenuDetails]    Script Date: 27-05-21 10:41:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MenuDetails](
	[DishId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
 CONSTRAINT [PK_MenuDetails] PRIMARY KEY CLUSTERED 
(
	[DishId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MenuDetails]  WITH CHECK ADD  CONSTRAINT [FK_MenuDetails_Dishes] FOREIGN KEY([DishId])
REFERENCES [dbo].[Dishes] ([DishId])
GO

ALTER TABLE [dbo].[MenuDetails] CHECK CONSTRAINT [FK_MenuDetails_Dishes]
GO

ALTER TABLE [dbo].[MenuDetails]  WITH CHECK ADD  CONSTRAINT [FK_MenuDetails_Menus] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([MenuId])
GO

ALTER TABLE [dbo].[MenuDetails] CHECK CONSTRAINT [FK_MenuDetails_Menus]
GO

