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

        void CalculatePlayerHoleStatistics(int month, int year);

        PlayerStatistics GetPlayerStatistics(int playerId, int month, int year);

        IEnumerable<PlayerStatistics> GetPlayerStatistics(int playerId);

        IEnumerable<PlayerStatistics> GetPlayerStatistics(int month, int year);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int month, int year);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId, int month, int year);

        PlayerCareerStatistics GetPlayerCareerStatistics(int playerId);

        IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int month, int year);

        IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int playerId, int month, int year);
    }
}