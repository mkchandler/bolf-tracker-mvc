USE BolfTracker

-- Average number of players per game
SELECT AVG(G.PlayerCount)
FROM (SELECT CONVERT(decimal, COUNT(PlayerId)) AS PlayerCount
	  FROM PlayerGameStatistics 
	  GROUP BY GameId) G

-- Average number of players per game (should match the above number)
SELECT AVG(CONVERT(decimal, PlayerCount)) AS AveragePlayerCount
FROM GameStatistics

