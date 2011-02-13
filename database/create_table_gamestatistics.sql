USE [BolfTracker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GameStatistics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Points] [int] NOT NULL,
	[Winner] [bit] NOT NULL,
	[ShotsMade] [int] NOT NULL,
	[Attempts] [int] NOT NULL,
	[Pushes] [int] NOT NULL,
	[Steals] [int] NOT NULL,
	[SugarFreeSteals] [int] NOT NULL,
 CONSTRAINT [PK_GameStatistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_GamePlayer] UNIQUE NONCLUSTERED 
(
	[GameId] ASC,
	[PlayerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GameStatistics]  WITH CHECK ADD  CONSTRAINT [FK_GameStatistics_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([Id])
GO

ALTER TABLE [dbo].[GameStatistics] CHECK CONSTRAINT [FK_GameStatistics_Game]
GO

ALTER TABLE [dbo].[GameStatistics]  WITH CHECK ADD  CONSTRAINT [FK_GameStatistics_Player] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Player] ([Id])
GO

ALTER TABLE [dbo].[GameStatistics] CHECK CONSTRAINT [FK_GameStatistics_Player]
GO