using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IShotRepository : IRepository<Shot>
    {
        IEnumerable<Shot> GetByGame(int gameId);

        IEnumerable<Shot> GetByMonthAndYear(int month, int year);

        IEnumerable<Shot> GetByGameAndPlayer(int gameId, int playerId);
    }
}
