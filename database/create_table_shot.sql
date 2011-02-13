USE [BolfTracker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Shot](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
	[HoleId] [int] NOT NULL,
	[ShotTypeId] [int] NOT NULL,
	[Attempts] [int] NOT NULL,
	[Points] [int] NOT NULL,
	[ShotMade] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Shot]  WITH CHECK ADD  CONSTRAINT [FK_Score_Game] FOREIGN KEY([GameId])
REFERENCES [dbo].[Game] ([Id])
GO

ALTER TABLE [dbo].[Shot] CHECK CONSTRAINT [FK_Score_Game]
GO

ALTER TABLE [dbo].[Shot]  WITH CHECK ADD  CONSTRAINT [FK_Score_Hole] FOREIGN KEY([HoleId])
REFERENCES [dbo].[Hole] ([Id])
GO

ALTER TABLE [dbo].[Shot] CHECK CONSTRAINT [FK_Score_Hole]
GO

ALTER TABLE [dbo].[Shot]  WITH CHECK ADD  CONSTRAINT [FK_Score_Player] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Player] ([Id])
GO

ALTER TABLE [dbo].[Shot] CHECK CONSTRAINT [FK_Score_Player]
GO

ALTER TABLE [dbo].[Shot]  WITH CHECK ADD  CONSTRAINT [FK_Score_ScoreType] FOREIGN KEY([ShotTypeId])
REFERENCES [dbo].[ShotType] ([Id])
GO

ALTER TABLE [dbo].[Shot] CHECK CONSTRAINT [FK_Score_ScoreType]
GO