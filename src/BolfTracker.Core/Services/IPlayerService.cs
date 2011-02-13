using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IPlayerService
    {
        Player GetPlayer(int id);

        IEnumerable<Player> GetPlayers();

        Player Create(string name);

        Player Update(int id, string name);

        void Delete(int id);
    }
}