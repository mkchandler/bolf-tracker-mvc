using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleStatisticsRepository : RepositoryBase<HoleStatistics>, IHoleStatisticsRepository
    {
        public HoleStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public override IEnumerable<HoleStatistics> All()
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string query =
                    "SELECT SUM(hs.ShotsMade) AS ShotsMade, " +
                    "SUM(hs.Attempts) AS Attempts, " +
                    "SUM(CONVERT(decimal,hs.ShotsMade)) / SUM(CONVERT(decimal,hs.Attempts)) AS ShootingPercentage, " +
                    "SUM(hs.PointsScored) AS PointsScored, SUM(hs.Pushes) AS Pushes, SUM(hs.Steals) AS Steals, " +
                    "SUM(hs.SugarFreeSteals) AS SugarFreeSteals " +
                    "FROM HoleStatistics hs INNER JOIN Hole h ON h.Id = hs.HoleId GROUP BY h.Id HAVING SUM(Attempts) > 0 ";

                var holeStatistics = connection.Query<HoleStatistics/*, Hole, HoleStatistics*/>(query/*, (hs, h) => { hs.Hole = h; return hs; }*/).ToList();

                return holeStatistics;
            }
        }

        public IEnumerable<HoleStatistics> GetByMonthAndYear(int month, int year)
        {
            var holeStatistics = Database.HoleStatistics.Include(hs => hs.Hole).Where(hs => hs.Month == month && hs.Year == year).ToList();

            return holeStatistics;
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            Database.ExecuteStoreCommand("DELETE FROM HoleStatistics WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
        }
    }
}