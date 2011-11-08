using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayersViewModel
    {
        public PlayersViewModel(IEnumerable<Player> players, IEnumerable<PlayerStatistics> playerStatistics)
        {
            Players = players;
            _playerStatistics = playerStatistics;
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