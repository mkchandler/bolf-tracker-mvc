/* Initial ShotType Data */

INSERT INTO [BolfTracker].[dbo].[ShotType]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (1
           ,'Make'
           ,'Player made the shot')
GO

INSERT INTO [BolfTracker].[dbo].[ShotType]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (2
           ,'Miss'
           ,'Player missed the shot')
GO

INSERT INTO [BolfTracker].[dbo].[ShotType]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (3
           ,'Push'
           ,'A player pushes the points to the next hole')
GO

INSERT INTO [BolfTracker].[dbo].[ShotType]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (4
           ,'Steal'
           ,'A player steals the points from another player')
GO

INSERT INTO [BolfTracker].[dbo].[ShotType]
           ([Id]
           ,[Name]
           ,[Description])
     VALUES
           (5
           ,'Sugar-Free Steal'
           ,'A player steals the points when the hole is already pushed')
GO