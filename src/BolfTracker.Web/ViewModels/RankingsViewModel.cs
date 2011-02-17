using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class RankingsViewModel
    {
        public RankingsViewModel(IEnumerable<Ranking> rankings)
        {
            _rankings = rankings;
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