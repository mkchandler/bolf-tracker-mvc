using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class GamesViewModel
    {
        public GamesViewModel(int month, int year, IEnumerable<Game> games, IEnumerable<PlayerGameStatistics> playerGameStatistics)
        {
            Month = month;
            Year = year;
            _games = games;
            _playerGameStatistics = playerGameStatistics;
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

        private IEnumerable<PlayerGameStatistics> _playerGameStatistics;

        public IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int gameId)
        {
            return _playerGameStatistics.Where(pgs => pgs.Game.Id == gameId).OrderByDescending(pgs => pgs.Points).ThenByDescending(pgs => pgs.ShootingPercentage);
        }
    }
}
