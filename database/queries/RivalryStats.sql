USE BolfTracker

--SELECT TOP 1000 prs.*
--FROM [BolfTracker].[dbo].[PlayerRivalryStatistics] prs
--inner join Game g on g.Id = prs.GameId
--where g.Date >= '2/1/2013'

SELECT p1.Name,
	   p2.Name AS Affected_Player,
	   COUNT(prs.AffectedPlayerId) AS Occurences,
	   SUM(prs.Points) AS Points
  FROM [BolfTracker].[dbo].[PlayerRivalryStatistics] prs
  inner join Player p1 on p1.Id = prs.PlayerId 
  inner join Player p2 on p2.Id = prs.AffectedPlayerId
  inner join Game g on g.Id = prs.GameId
  where prs.AffectedPlayerId = 28
  and prs.ShotTypeId in (4,5)
  --and g.Date >= '2/1/2013'
  --and prs.HoleId >= 10
  group by p1.Name, p2.Name
  order by 3 desc
  
  select * from player
  /******
1	Make
2	Miss
3	Push
4	Steal
5	Sugar-Free Steal
******/
  --select * from shottype
  --select * from PlayerRivalryStatistics
  
