using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerHoleStatisticsRepository : IRepository<PlayerHoleStatistics>
    {
        PlayerHoleStatistics GetByPlayerHoleMonthAndYear(int playerId, int holeId, int month, int year);

        IEnumerable<PlayerHoleStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year);

        IEnumerable<PlayerHoleStatistics> GetByMonthAndYear(int month, int year);
    }
}