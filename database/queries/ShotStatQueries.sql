
select * from PlayerGameStatistics pgs
inner join GameStatistics gs
on gs.GameId = pgs.GameId
 where pgs.ShootingPercentage >= 0.500
 order by pgs.ShootingPercentage desc
 
 
 
select pgs.PlayerId, COUNT(*) from PlayerGameStatistics pgs
inner join GameStatistics gs
on gs.GameId = pgs.GameId
 where pgs.ShotsMade >= 10
 group by pgs.PlayerId


 
select pgs.PlayerId, COUNT(*) from PlayerGameStatistics pgs
inner join GameStatistics gs
on gs.GameId = pgs.GameId
 where pgs.ShootingPercentage >= 0.500
 group by pgs.PlayerId
 order by 2 desc


select pgs.PlayerId, COUNT(*) 
from PlayerGameStatistics pgs
inner join GameStatistics gs
on gs.GameId = pgs.GameId
where pgs.PlayerId = 28 and pgs.Winner = 1
group by pgs.PlayerId
order by 2 desc

-- Determine a player's EFG%
SELECT SUM(TotalAttempts) AS TotalAttempts
FROM (
	SELECT H.Par, SUM(PGS.ShotsMade) AS TotalShotsMade, SUM(PGS.Attempts) AS TotalAttempts
	FROM PlayerHoleStatistics PGS
	INNER JOIN Hole H ON H.Id = PGS.HoleId
	WHERE [Month] = 1 AND [Year] = 2014 AND PlayerId = 28
	GROUP BY H.Par) AS GS

