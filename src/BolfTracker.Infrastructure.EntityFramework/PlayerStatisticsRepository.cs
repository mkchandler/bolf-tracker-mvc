using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerStatisticsRepository : RepositoryBase<PlayerStatistics>, IPlayerStatisticsRepository
    {
        public PlayerStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public PlayerStatistics GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            var playerStatistics = Database.PlayerStatistics.First(ps => ps.Player.Id == playerId && ps.Month == month && ps.Year == year);

            return playerStatistics;
        }

        public IEnumerable<PlayerStatistics> GetByPlayer(int playerId)
        {
            var playerStatistics = Database.PlayerStatistics.Where(ps => ps.Player.Id == playerId).ToList();

            return playerStatistics;
        }

        public IEnumerable<PlayerStatistics> GetByMonthAndYear(int month, int year)
        {
            var playerStatistics = Database.PlayerStatistics.Where(ps => ps.Month == month && ps.Year == year).ToList();

            return playerStatistics;
        }
    }
}