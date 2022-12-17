USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[Opinions]    Script Date: 27-05-21 10:41:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Opinions](
	[OpinionId] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OpinionText] [nvarchar](400) NOT NULL,
	[Rating] [int] NOT NULL,
 CONSTRAINT [PK_Opinions_1] PRIMARY KEY CLUSTERED 
(
	[OpinionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Opinions]  WITH CHECK ADD  CONSTRAINT [FK_Opinions_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Opinions] CHECK CONSTRAINT [FK_Opinions_Customers]
GO

ALTER TABLE [dbo].[Opinions]  WITH CHECK ADD  CONSTRAINT [FK_Opinions_Restaurants] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([RestaurantId])
GO

ALTER TABLE [dbo].[Opinions] CHECK CONSTRAINT [FK_Opinions_Restaurants]
GO

