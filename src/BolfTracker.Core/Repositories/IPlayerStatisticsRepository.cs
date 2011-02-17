using BolfTracker.Models;
using System.Collections.Generic;

namespace BolfTracker.Repositories
{
    public interface IPlayerStatisticsRepository : IRepository<PlayerStatistics>
    {
        PlayerStatistics GetByPlayerAndMonth(int playerId, int month, int year);

        IEnumerable<PlayerStatistics> GetByPlayer(int playerId);

        IEnumerable<PlayerStatistics> GetByMonthAndYear(int month, int year);
    }
}