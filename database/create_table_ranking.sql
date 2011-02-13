USE [BolfTracker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ranking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
	[WinningPercentage] [decimal](18, 3) NOT NULL,
	[TotalPoints] [int] NOT NULL,
	[PointsPerGame] [int] NOT NULL,
	[GamesBack] [decimal](18, 1) NOT NULL,
	[LastTenWins] [int] NOT NULL,
	[LastTenLosses] [int] NOT NULL,
	[LastTenWinningPercentage] [decimal](18, 3) NOT NULL,
	[Eligible] [bit] NOT NULL,
 CONSTRAINT [PK_Ranking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Ranking]  WITH CHECK ADD  CONSTRAINT [FK_Ranking_Player] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Player] ([Id])
GO

ALTER TABLE [dbo].[Ranking] CHECK CONSTRAINT [FK_Ranking_Player]
GO