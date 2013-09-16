using System;
using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IGameService
    {
        Game GetGame(int id);

        IEnumerable<Game> GetGames(int month, int year);

        Game CreateGame(DateTime date);

        Game UpdateGame(int id, DateTime date);

        void DeleteGame(int id);

        void CalculateGameStatistics();

        void CalculateGameStatistics(int gameId);

        void CalculateGameStatistics(int month, int year);

        void DeleteGameStatistics(int gameId);

        void CalculatePlayerRivalryStatistics();

        void CalculatePlayerRivalryStatistics(int gameId);

        void CalculatePlayerRivalryStatistics(int month, int year);

        void DeletePlayerRivalryStatistics(int gameId);
    }
}
