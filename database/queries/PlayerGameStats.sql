
--select TOP 100 * from GameStatistics

SELECT COUNT(DISTINCT(GameId)) 
FROM PlayerGameStatistics

SELECT COUNT(*)
FROM GameStatistics

select * from Game where Id = 1118
select * from GameStatistics where GameId = 1118
select * from PlayerGameStatistics where GameId = 1118

select GameId, COUNT(*)
from GameStatistics
Group by GameId
having COUNT(*) > 1

select gs.* from GameStatistics gs
where gs.GameId NOT IN (SELECT DISTINCT(GameId) FROM PlayerGameStatistics)

-- Number of games played by player
SELECT PlayerId, COUNT(*) 
FROM PlayerGameStatistics
GROUP BY PlayerId
ORDER BY 2 DESC

-- Average number of players per game
SELECT AVG(CONVERT(decimal, PlayerCount)) AS AveragePlayerCount
FROM GameStatistics

-- Average number of players per game by player
SELECT PGS.PlayerId, P.Name, AVG(CONVERT(decimal, GS.PlayerCount)) AS AveragePlayerCount
FROM GameStatistics GS
INNER JOIN PlayerGameStatistics PGS ON PGS.GameId = GS.GameId
INNER JOIN Player P ON P.Id = PGS.PlayerId
GROUP BY PGS.PlayerId, P.Name
HAVING COUNT(PGS.PlayerId) > 100
ORDER BY 3 ASC

-- Player shooting percentage by number of players in a game
SELECT GS.PlayerCount, STR(100.0 * CONVERT(decimal(18, 3),AVG(PGS.ShootingPercentage)), 4, 1) AS ShootingPercentage
FROM PlayerGameStatistics PGS
INNER JOIN GameStatistics GS ON GS.GameId = PGS.GameId
WHERE PGS.PlayerId = 28
GROUP BY GS.PlayerCount

-- Player shooting percentage by number of players in a game (with Jeaux)
SELECT PGS.PlayerId, COUNT(*) AS Games, (SUM(CONVERT(decimal, PGS.ShotsMade)) / SUM(CONVERT(decimal, PGS.Attempts))) AS ShootingPercentage
FROM (SELECT *
	  FROM PlayerGameStatistics 
	  WHERE GameId IN(SELECT GameId
		              FROM PlayerGameStatistics 
		              WHERE PlayerId = 29)) PGS
WHERE PGS.PlayerId = 54
GROUP BY PGS.PlayerId

-- Player shooting percentage by number of players in a game (without Jeaux)
SELECT PGS.PlayerId, COUNT(*) AS Games, (SUM(CONVERT(decimal, PGS.ShotsMade)) / SUM(CONVERT(decimal, PGS.Attempts))) AS ShootingPercentage
FROM (SELECT *
	  FROM PlayerGameStatistics 
	  WHERE GameId NOT IN(SELECT GameId
		                  FROM PlayerGameStatistics 
		                  WHERE PlayerId = 29)) PGS
WHERE PGS.PlayerId = 54
GROUP BY PGS.PlayerId
