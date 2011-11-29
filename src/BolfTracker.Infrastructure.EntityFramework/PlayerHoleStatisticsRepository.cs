using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerHoleStatisticsRepository : RepositoryBase<PlayerHoleStatistics>, IPlayerHoleStatisticsRepository
    {
        public PlayerHoleStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public PlayerHoleStatistics GetByPlayerHoleMonthAndYear(int playerId, int holeId, int month, int year)
        {
            var playerHoleStatistics = Database.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).First(phs => phs.Player.Id == playerId && phs.Hole.Id == holeId && phs.Month == month && phs.Year == year);

            return playerHoleStatistics;
        }

        public IEnumerable<PlayerHoleStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            var playerHoleStatistics = Database.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).Where(phs => phs.Player.Id == playerId && phs.Month == month && phs.Year == year).ToList();

            return playerHoleStatistics;
        }

        public IEnumerable<PlayerHoleStatistics> GetByMonthAndYear(int month, int year)
        {
            var playerHoleStatistics = Database.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).Where(phs => phs.Month == month && phs.Year == year).ToList();

            return playerHoleStatistics;
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            Database.ExecuteStoreCommand("DELETE FROM PlayerHoleStatistics WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
        }
    }
}