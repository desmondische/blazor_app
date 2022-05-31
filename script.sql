USE [MarketDb]
GO
/****** Object:  Table [dbo].[IceElectric2021]    Script Date: 31.05.2022 15:48:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IceElectric2021](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price hub] [varchar](50) NOT NULL,
	[Trade date] [datetime] NOT NULL,
	[Delivery start date] [datetime] NOT NULL,
	[Delivery end date] [datetime] NOT NULL,
	[High price $/MWh] [float] NOT NULL,
	[Low price $/MWh] [float] NOT NULL,
	[Wtd avg price $/MWh] [float] NOT NULL,
	[Change] [float] NOT NULL,
	[Daily volume MWh] [int] NOT NULL,
 CONSTRAINT [PK_IceElectric2021] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceHubs]    Script Date: 31.05.2022 15:48:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceHubs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PriceHubs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
