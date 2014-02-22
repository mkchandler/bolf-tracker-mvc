﻿using System.Collections.Generic;

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
        IEnumerable<PlayerBadges> GetPlayerBadges(int playerId);
        void CalculatePlayerStatistics();

        void CalculatePlayerStatistics(int month, int year, bool calculateCareerStatistics);

        PlayerStatistics GetPlayerStatistics(int playerId, int month, int year);

        IEnumerable<PlayerStatistics> GetPlayerStatistics(int playerId);

        IEnumerable<PlayerStatistics> GetPlayerStatistics(int month, int year);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int month, int year);

        IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId, int month, int year);

        PlayerCareerStatistics GetPlayerCareerStatistics(int playerId);

        IEnumerable<PlayerCareerStatistics> GetPlayerCareerStatistics();

        IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int month, int year);

        IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int playerId, int month, int year);
    }
}
