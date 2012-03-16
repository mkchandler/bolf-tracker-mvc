using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IShotRepository
    {
        Shot GetById(int id);
        
        IEnumerable<Shot> GetByGame(int gameId);

        IEnumerable<Shot> GetByMonthAndYear(int month, int year);

        IEnumerable<Shot> GetByGameAndPlayer(int gameId, int playerId);

        IEnumerable<Shot> All();

        void Add(Shot model);

        void Delete(int id);
    }
}
