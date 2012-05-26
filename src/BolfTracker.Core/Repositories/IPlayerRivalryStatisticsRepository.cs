using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerRivalryStatisticsRepository : IRepository<PlayerRivalryStatistics>
    {
        IEnumerable<PlayerRivalryStatistics> GetByPlayer(int playerId);

        void DeleteAll();

        void DeleteByMonthAndYear(int month, int year);

        void DeleteByGame(int gameId);
    }
}
