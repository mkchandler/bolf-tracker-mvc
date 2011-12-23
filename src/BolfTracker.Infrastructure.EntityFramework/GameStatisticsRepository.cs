using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameStatisticsRepository : RepositoryBase<GameStatistics>, IGameStatisticsRepository
    {
        public GameStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public void DeleteAll()
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE FROM GameStatistics";

                connection.Execute(command);
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE GameStatistics FROM GameStatistics gs INNER JOIN Game g ON g.Id = gs.GameId WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                connection.Execute(command, new { Month = month, Year = year });
            }
        }
    }
}
