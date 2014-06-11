using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayerViewModel
    {
        public PlayerViewModel(int month, int year, Player player, IEnumerable<PlayerStatistics> playerStatistics, PlayerCareerStatistics playerCareerStatistics, IEnumerable<PlayerHoleStatistics> playerHoleStatistics,IEnumerable<PlayerBadges> playerBadges)
        {
            Month = month;
            Year = year;
            Player = player;
            _playerStatistics = playerStatistics;
            PlayerCareerStatistics = playerCareerStatistics;
            PlayerHoleStatistics = playerHoleStatistics;
            _playerBadges = playerBadges;
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

        public Player Player
        {
            get;
            private set;
        }

        private IEnumerable<PlayerStatistics> _playerStatistics;

        public IEnumerable<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics.OrderByDescending(ps => ps.Year).ThenByDescending(ps => ps.Month); }
        }

        public PlayerCareerStatistics PlayerCareerStatistics
        {
            get;
            private set;
        }

        public IEnumerable<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get;
            private set;
        }

        IEnumerable<PlayerBadges> _playerBadges;
        public IEnumerable<PlayerBadges> PlayerBadges
        {
            get { return _playerBadges; }
        }
    }
}
