using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class GamesViewModel
    {
        public GamesViewModel(IEnumerable<Game> games)
        {
            _games = games;
        }

        private IEnumerable<Game> _games;

        public IEnumerable<Game> Games
        {
            get { return _games; }
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