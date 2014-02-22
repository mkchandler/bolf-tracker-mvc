
-- Average ranking by player
SELECT PlayerId, P.Name, AVG(CONVERT(decimal(18, 3), Ranking)) AS AverageRanking
FROM (
	SELECT RANK() OVER (PARTITION BY [Year], [Month] ORDER BY WinningPercentage DESC, PointsPerGame DESC) AS Ranking, [Year], [Month], PlayerId
	FROM Ranking 
	WHERE Eligible = 1
	) R
INNER JOIN Player P ON P.Id = R.PlayerId
GROUP BY PlayerId, P.Name
ORDER BY 3 ASC

-- Ranking per month by player
SELECT PlayerId, P.Name, [Year], [Month], Ranking
FROM (
	SELECT RANK() OVER (PARTITION BY [Year], [Month] ORDER BY WinningPercentage DESC, PointsPerGame DESC) AS Ranking, [Year], [Month], PlayerId
	FROM Ranking 
	WHERE Eligible = 1
	) R
INNER JOIN Player P ON P.Id = R.PlayerId
WHERE R.PlayerId = 31

