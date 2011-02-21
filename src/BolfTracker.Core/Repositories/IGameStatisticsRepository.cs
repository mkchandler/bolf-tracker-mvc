using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IGameStatisticsRepository : IRepository<GameStatistics>
    {
        IEnumerable<GameStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year);
    }
}