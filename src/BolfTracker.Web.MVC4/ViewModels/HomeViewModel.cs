using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class HomeViewModel
    {
        public HomeViewModel(IEnumerable<Ranking> latestMonthRankings, IEnumerable<PlayerStatistics> latestMonthPlayerStatistics)
        {
            _latestMonthRankings = latestMonthRankings;
            _latestMonthPlayerStatistics = latestMonthPlayerStatistics;
        }

        public string LatestMonthDisplay
        {
            get
            {
                var rankingRecord = _latestMonthRankings.FirstOrDefault();

                if (rankingRecord != null)
                {
                    return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(rankingRecord.Month) + " " + rankingRecord.Year;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        private IEnumerable<Ranking> _latestMonthRankings;

        public IEnumerable<Ranking> LatestMonthRankings
        {
            get
            {
                return _latestMonthRankings.Where(r => r.Eligible)
                                           .OrderByDescending(r => r.WinningPercentage)
                                           .ThenByDescending(r => r.PointsPerGame)
                                           .ThenByDescending(r => r.TotalPoints);
            }
        }

        private IEnumerable<PlayerStatistics> _latestMonthPlayerStatistics;

        public IEnumerable<PlayerStatistics> LatestMonthPlayerStatistics
        {
            get
            {
                return _latestMonthPlayerStatistics.OrderByDescending(ps => ps.ShootingPercentage);
            }
        }
    }
}
