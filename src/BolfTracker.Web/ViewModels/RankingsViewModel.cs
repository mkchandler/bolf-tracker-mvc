using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class RankingsViewModel
    {
        public RankingsViewModel(int month, int year, IEnumerable<Ranking> rankings)
        {
            Month = month;
            Year = year;
            _rankings = rankings;
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

        private IEnumerable<Ranking> _rankings;

        public IEnumerable<Ranking> Rankings
        {
            get
            {
                return _rankings.Where(r => r.Eligible)
                                .OrderByDescending(r => r.WinningPercentage)
                                .ThenByDescending(r => r.PointsPerGame)
                                .ThenByDescending(r => r.TotalPoints);
            }
        }

        public IEnumerable<Ranking> IneligibleRankings
        {
            get
            {
                return _rankings.Where(r => !r.Eligible)
                                .OrderByDescending(r => r.WinningPercentage)
                                .ThenByDescending(r => r.PointsPerGame)
                                .ThenByDescending(r => r.TotalPoints);
            }
        }
    }
}