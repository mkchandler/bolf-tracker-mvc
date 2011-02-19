using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayersViewModel
    {
        public PlayersViewModel(IEnumerable<Player> players, IEnumerable<PlayerStatistics> playerStatistics)
        {
            Players = players;
            PlayerStatistics = playerStatistics;
        }

        public IEnumerable<Player> Players
        {
            get;
            private set;
        }

        public IEnumerable<PlayerStatistics> PlayerStatistics
        {
            get;
            private set;
        }
    }
}