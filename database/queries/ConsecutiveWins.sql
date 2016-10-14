--SELECT	*,
--		ROW_NUMBER() OVER(ORDER BY GameId ASC) ROWID
--FROM	PlayerGameStatistics
--WHERE PlayerId = 28

;WITH Vals AS (
	SELECT *,
		   ROW_NUMBER() OVER(ORDER BY GameId ASC) ROWID
	FROM PlayerGameStatistics
	WHERE PlayerId = 30
)
, ValsNext AS (
		SELECT	v.PlayerId,
				v.Winner,
				v.ROWID,
				MIN(vn.ROWID) NextRowID
		FROM	Vals v LEFT JOIN
				Vals vN ON	v.PlayerId = vn.PlayerId
						AND v.Winner != vn.Winner
						AND v.ROWID < vn.ROWID
		GROUP BY	v.PlayerId,
					v.Winner,
					v.ROWID
)
, ValDiffs AS (
		SELECT	vn.PlayerId,
				vn.Winner,
				vn.RowID,
				vn.NextRowID,
				vn.NextRowID - vn.ROWID Consecutive
		FROM	ValsNext vn
)
, Players AS (
		SELECT PlayerId,
			   MAX(Consecutive) MaxConsecutive
		FROM ValDiffs
		GROUP BY PlayerId
)

--SELECT * from ValDiffs
--WHERE Winner = 1
--ORDER BY Consecutive DESC

SELECT	vd.*
FROM	Players s INNER JOIN
		ValDiffs vd	ON	s.PlayerId = vd.PlayerId
					--AND	s.MaxConsecutive = vd.Consecutive
WHERE vd.Winner = 0
AND vd.Consecutive IS NOT NULL 
AND vd.Consecutive > 1
ORDER BY vd.Consecutive DESC
