/* Create Database */

USE [master]
GO

CREATE DATABASE [BolfTracker] ON  PRIMARY 
( NAME = N'BolfTracker', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\BolfTracker.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BolfTracker_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\BolfTracker_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [BolfTracker] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BolfTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [BolfTracker] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [BolfTracker] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [BolfTracker] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [BolfTracker] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [BolfTracker] SET ARITHABORT OFF 
GO

ALTER DATABASE [BolfTracker] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [BolfTracker] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [BolfTracker] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [BolfTracker] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [BolfTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [BolfTracker] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [BolfTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [BolfTracker] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [BolfTracker] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [BolfTracker] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [BolfTracker] SET  DISABLE_BROKER 
GO

ALTER DATABASE [BolfTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [BolfTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [BolfTracker] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [BolfTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [BolfTracker] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [BolfTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [BolfTracker] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [BolfTracker] SET  READ_WRITE 
GO

ALTER DATABASE [BolfTracker] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [BolfTracker] SET  MULTI_USER 
GO

ALTER DATABASE [BolfTracker] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [BolfTracker] SET DB_CHAINING OFF 
GO

/* Create Tables */

USE [BolfTracker]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Player](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Hole](
	[Id] [int] NOT NULL,
	[Par] [int] NOT NULL,
 CONSTRAINT [PK_Hole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Game](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[ShotType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ScoreType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

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

CREATE TABLE [dbo].[PlayerStatistics](
	[Id] [int] NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[ShotsMade] [int] NOT NULL,
	[Attempts] [int] NOT NULL,
	[Wins] [int] NOT NULL,
	[Losses] [int] NOT NULL,
 CONSTRAINT [PK_PlayerStatistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PlayerStatistics]  WITH CHECK ADD  CONSTRAINT [FK_PlayerStatistics_Player] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Player] ([Id])
GO

ALTER TABLE [dbo].[PlayerStatistics] CHECK CONSTRAINT [FK_PlayerStatistics_Player]
GO

SET ANSI_PADDING OFF
GO