USE [ASP_Project_02]
GO

/****** Object:  Table [dbo].[Restorers]    Script Date: 27-05-21 10:42:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Restorers](
	[RestorerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Gender] [nvarchar](1) NOT NULL,
	[City] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[PostalCode] [nvarchar](20) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Restorers] PRIMARY KEY CLUSTERED 
(
	[RestorerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

