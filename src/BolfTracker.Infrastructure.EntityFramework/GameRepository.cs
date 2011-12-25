using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameRepository : RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<Game> GetAllFinalized()
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT g.* FROM Game g INNER JOIN GameStatistics gs ON gs.GameId = g.Id";

                var games = connection.Query<Game>(query).ToList();

                return games;
            }
        }

        public IEnumerable<Game> GetByMonthAndYear(int month, int year)
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Game WHERE (DATEPART (month, [Date])) = @Month AND (DATEPART (year, [Date])) = @Year";

                var games = connection.Query<Game>(query, new { Month = month, Year = year }).ToList();

                return games;
            }
        }

        public IEnumerable<Game> GetFinalizedByMonthAndYear(int month, int year)
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT g.* FROM Game g INNER JOIN GameStatistics gs ON gs.GameId = g.Id WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                var games = connection.Query<Game>(query, new { Month = month, Year = year }).ToList();

                return games;
            }
        }
    }
}
