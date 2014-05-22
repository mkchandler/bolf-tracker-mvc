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

        void Add(Shot shot);

        void Update(Shot shot);

        void Delete(int id);

        void DeleteToShot(int gameId, int shotId);
    }
}
