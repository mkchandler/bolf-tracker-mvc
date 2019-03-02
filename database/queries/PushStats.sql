
-- Pushes by total attempts
SELECT Attempts,
	   COUNT(Attempts) AS TotalPushes
FROM Shot
WHERE ShotTypeId = 3 
	AND ShotMade = 1
GROUP BY Attempts
ORDER BY 2 DESC

-- Pushes by hole (old course)
SELECT s.HoleId,
	   COUNT(s.HoleId) AS TotalPushes
FROM Shot s
INNER JOIN Game g ON g.Id = s.GameId
INNER JOIN Hole h ON h.Id = s.HoleId
WHERE s.ShotTypeId = 3 
	AND s.ShotMade = 1
	AND DATEPART(YEAR, g.[Date]) < 2016
GROUP BY s.HoleId
ORDER BY 2 DESC

-- Pushes by hole (new course)
SELECT s.HoleId,
	   COUNT(s.HoleId) AS TotalPushes
FROM Shot s
INNER JOIN Game g ON g.Id = s.GameId
INNER JOIN Hole h ON h.Id = s.HoleId
WHERE s.ShotTypeId = 3 
	AND s.ShotMade = 1
	AND DATEPART(YEAR, g.[Date]) >= 2016
GROUP BY s.HoleId
ORDER BY 2 DESC

SELECT s.HoleId,
	   COUNT(s.HoleId) AS CountPointsScored
FROM Shot s
INNER JOIN Game g ON g.Id = s.GameId
WHERE s.Points > 0
	AND DATEPART(YEAR, g.[Date]) >= 2016
GROUP BY s.HoleId
ORDER BY 2 DESC
