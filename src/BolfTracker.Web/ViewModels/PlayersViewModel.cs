using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayersViewModel
    {
        public PlayersViewModel(int month, int year, IEnumerable<Player> players, IEnumerable<PlayerStatistics> playerStatistics)
        {
            Month = month;
            Year = year;
            Players = players;
            _playerStatistics = playerStatistics;
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

        public IEnumerable<Player> Players
        {
            get;
            private set;
        }

        private IEnumerable<PlayerStatistics> _playerStatistics;

        public IEnumerable<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics.OrderByDescending(ps => ps.ShootingPercentage); }
        }
    }
}