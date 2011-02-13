using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IShotService
    {
        IEnumerable<Shot> GetShots(int gameId);

        Shot GetShot(int id);

        void Create(int gameId, int playerId, int holeId, int attempts, bool shotMade);

        void Update(int id, int points, ShotType shotType);

        void Delete(int id);
    }
}