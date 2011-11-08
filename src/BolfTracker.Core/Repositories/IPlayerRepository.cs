using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Player GetByName(string name);

        IEnumerable<Player> GetByGame(int gameId);

        IEnumerable<Player> GetActiveByMonthAndYear(int month, int year);
    }
}