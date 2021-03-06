USE [BolfTracker]

-- Row counts by table
    SELECT sysobjects.Name
         , sysindexes.Rows
      FROM sysobjects
INNER JOIN sysindexes
        ON sysobjects.id = sysindexes.id
	 WHERE type = 'U'
	   AND sysindexes.IndId < 2
  ORDER BY sysobjects.Name

-- Total row count in database
    SELECT SUM(sysindexes.Rows) AS Rows
      FROM sysindexes
INNER JOIN sysobjects
        ON sysobjects.id = sysindexes.id
	 WHERE type = 'U'
	   AND sysindexes.IndId < 2
