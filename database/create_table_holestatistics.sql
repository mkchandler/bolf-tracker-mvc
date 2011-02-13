USE [BolfTracker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HoleStatistics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HoleId] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Makes] [int] NOT NULL,
	[Misses] [int] NOT NULL,
 CONSTRAINT [PK_HoleStatistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[HoleStatistics]  WITH CHECK ADD  CONSTRAINT [FK_HoleStatistics_Hole] FOREIGN KEY([HoleId])
REFERENCES [dbo].[Hole] ([Id])
GO

ALTER TABLE [dbo].[HoleStatistics] CHECK CONSTRAINT [FK_HoleStatistics_Hole]
GO