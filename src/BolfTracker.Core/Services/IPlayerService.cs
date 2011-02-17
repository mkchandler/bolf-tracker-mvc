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

        void CalculatePlayerStatistics(int month, int year);

        void CalculatePlayerStatistics(int playerId, int month, int year);

        PlayerStatistics GetPlayerStatistics(int playerId, int month, int year);

        IEnumerable<PlayerStatistics> GetPlayersStatistics(int playerId);

        IEnumerable<PlayerStatistics> GetPlayersStatistics(int month, int year);
    }
}