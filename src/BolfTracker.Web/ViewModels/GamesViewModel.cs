using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class GamesViewModel
    {
        public GamesViewModel(int month, int year, IEnumerable<Game> games)
        {
            Month = month;
            Year = year;
            _games = games;
        }

        public int Month
        {
            get;
            private set;
        }

        public string MonthName
        {
            get
            {
                return new DateTime(1, Month, 1).ToString("MMMM");
            }
        }

        public int Year
        {
            get;
            private set;
        }

        private IEnumerable<Game> _games;

        public IEnumerable<Game> Games
        {
            get { return _games.OrderByDescending(game => game.Date); }
        }

        public IEnumerable<GameStatistics> GameStatistics
        {
            get { return _games.SelectMany(game => game.GameStatistics); }
        }

        public IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int gameId)
        {
            var playerGameStatistics = _games.Where(g => g.Id == gameId).SelectMany(g => g.PlayerGameStatistics).OrderByDescending(pgs => pgs.Points);

            return playerGameStatistics;
        }
    }
}