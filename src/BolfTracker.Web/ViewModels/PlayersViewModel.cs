using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayersViewModel
    {
        public PlayersViewModel(int month, int year, IEnumerable<PlayerStatistics> playerStatistics, IEnumerable<PlayerCareerStatistics> playerCareerStatistics)
        {
            Month = month;
            Year = year;
            _playerStatistics = playerStatistics;
            _playerCareerStatistics = playerCareerStatistics;
      
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

        private IEnumerable<PlayerStatistics> _playerStatistics;

        public IEnumerable<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics.OrderByDescending(ps => ps.ShootingPercentage); }
        }

        private IEnumerable<PlayerCareerStatistics> _playerCareerStatistics;

        public IEnumerable<PlayerCareerStatistics> PlayerCareerStatistics
        {
            get { return _playerCareerStatistics.OrderByDescending(ps => ps.ShootingPercentage); }
        }

       
    }
}
