SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetEligibilityLine] 
	@Month int, 
	@Year int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @PlayersToSample int

	SET @PlayersToSample = (SELECT (COUNT(DISTINCT(gs.PlayerId)) / 2) 
							FROM PlayerGameStatistics gs
							INNER JOIN Game g ON g.Id = gs.GameId
							WHERE DATEPART(M, g.Date) = @Month AND DATEPART(YYYY, g.Date) = @Year)

	IF (@PlayersToSample > 0)
	BEGIN
		SET ROWCOUNT @PlayersToSample 

		SELECT ((SUM(a.GameCount) / (@PlayersToSample + 1)) / 2) AS EligibilityLine
		FROM (SELECT TOP 100 PERCENT COUNT(gs.Id) AS GameCount 
			  FROM PlayerGameStatistics gs
			  INNER JOIN Game g ON g.Id = gs.GameId
			  WHERE DATEPART(M, g.Date) = @Month AND DATEPART(YYYY, g.Date) = @Year
			  GROUP BY gs.PlayerId
			  ORDER BY 1 DESC ) a

		SET ROWCOUNT 0
	END
	ELSE
	BEGIN
		SELECT 0
	END
END

GO
