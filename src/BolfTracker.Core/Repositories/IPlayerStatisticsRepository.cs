using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerStatisticsRepository : IRepository<PlayerStatistics>
    {
        PlayerStatistics GetByPlayerMonthAndYear(int playerId, int month, int year);

        IEnumerable<PlayerStatistics> GetByPlayer(int playerId);

        IEnumerable<PlayerStatistics> GetByMonthAndYear(int month, int year);
    }
}