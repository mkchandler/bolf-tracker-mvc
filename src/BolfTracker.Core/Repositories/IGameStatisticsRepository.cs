using BolfTracker.Models;
using System.Collections.Generic;

namespace BolfTracker.Repositories
{
    public interface IGameStatisticsRepository : IRepository<GameStatistics>
    {
        IEnumerable<GameStatistics> GetByPlayerAndMonth(int playerId, int month, int year);
    }
}