USE BolfTracker

-- "Get Lucky" - Win a game with the lowest shooting percentage
SELECT * FROM PlayerGameStatistics
WHERE Winner = 1

-- "Get Lucky" - Get lowest shooting percentage of each game where the player won
SELECT PGS.GameId, PGS.PlayerId, P.Name
FROM (
	SELECT RANK() OVER (PARTITION BY GameId ORDER BY ShootingPercentage ASC) AS Ranking, GameId, PlayerId, ShootingPercentage, Winner
	FROM PlayerGameStatistics) PGS
INNER JOIN Player P ON P.Id = PGS.PlayerId
WHERE PGS.Ranking = 1 AND PGS.Winner = 1 
ORDER BY PGS.GameId DESC

-- "Get Lucky" - Count the number of times each player has won with the lowest shooting percentage
SELECT P.Name, COUNT(PGS.PlayerId) AS BadgeCount
FROM (
	SELECT RANK() OVER (PARTITION BY GameId ORDER BY ShootingPercentage ASC) AS Ranking, GameId, PlayerId, ShootingPercentage, Winner
	FROM PlayerGameStatistics) PGS
INNER JOIN Player P ON P.Id = PGS.PlayerId
WHERE PGS.Ranking = 1 AND PGS.Winner = 1 
GROUP BY P.Name, PGS.PlayerId
ORDER BY 2 DESC

-- "Shutout" - Win a game in a shutout
SELECT * 
FROM PlayerGameStatistics 
WHERE Shutout = 1

-- "Overtime Shutout" - Win a game in a shutout in overtime
SELECT * 
FROM PlayerGameStatistics 
WHERE Shutout = 1 AND OvertimeWin = 1

-- "Pusher" - 5+ pushes in a game
SELECT * 
FROM PlayerGameStatistics
WHERE Pushes >= 5

-- "Stealer" - 3+ steals in a game
SELECT * 
FROM PlayerGameStatistics
WHERE Steals >= 3

