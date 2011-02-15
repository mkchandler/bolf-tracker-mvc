using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class GamePanelViewModel
    {
        public GamePanelViewModel(Game game, IEnumerable<ShotType> scoreTypes, IEnumerable<Player> allPlayers, IEnumerable<Hole> allHoles)
        {
            Game = game;

            _allPlayers = allPlayers;
            _allHoles = allHoles;
            _scoreTypes = scoreTypes;
        }

        public Game Game
        {
            get;
            private set;
        }

        public bool GameFinalized
        {
            get
            {
                return Game.GameStatistics.Any();
            }
        }

        public IEnumerable<Player> ActivePlayers
        {
            get
            {
                var players = new List<Player>();

                foreach (var score in Game.Shots)
                {
                    players.Add(score.Player);
                }

                return players;
            }
        }

        private Player GetCurrentPlayer()
        {
            var playerResult = new Player();

            if (Game.Shots.Any())
            {
                var distinctPlayers = Game.Shots.Select(s => s.Player).Distinct();
                var playersDescending = Game.Shots.OrderByDescending(s => s.Id).Take(distinctPlayers.Count()).Select(s => s.Player);
                var duplicatePlayers = Game.Shots.GroupBy(s => s.Player.Id).Where(p => p.Count() > 1);

                // Check to see if we've had any duplicate players yet (if so, that means we can determine the order)
                if (duplicatePlayers.Any())
                {
                    // If we are on the last hole or in overtime, the order could change because not
                    // everyone can win
                    if (GetCurrentHole() >= 10)
                    {

                    }

                    return playersDescending.Last();
                }
                else
                {
                    // If we can't determine the order, just get the next player who has not gone already
                    foreach (var player in _allPlayers)
                    {
                        if (!distinctPlayers.Contains(player))
                        {
                            return player;
                        }
                    }
                }
            }

            return playerResult;
        }

        private int GetCurrentHole()
        {
            int currentHole = 1;

            if (Game.Shots.Any())
            {
                currentHole = Game.Shots.Max(s => s.Hole.Id);

                var holeShots = Game.Shots.Where(s => s.Hole.Id == currentHole);

                if (currentHole == 1)
                {
                    // If we are on the first hole, there's no way of knowing when to go to the next hole
                    // because we don't know how many players are going (unless it's been pushed on 1)
                    if (holeShots.Count(s => s.Attempts == 1 && s.ShotMade == true) > 1)
                    {
                        currentHole += 1;
                        return currentHole;
                    }
                    else
                    {
                        return currentHole;
                    }
                }
                else
                {
                    // If the hole was pushed on 1, go to the next hole
                    if (holeShots.Count(s => s.Attempts == 1 && s.ShotMade == true) > 1)
                    {
                        currentHole += 1;
                        return currentHole;
                    }
                    
                    var totalPlayers = ActivePlayers.Distinct().Count();

                    // If everyone has gone, go to the next hole
                    if (holeShots.Count() == totalPlayers)
                    {
                        currentHole += 1;
                        return currentHole;
                    }
                }
            }

            return currentHole;
        }

        public int PointsAvailable
        {
            get
            {
                int currentHole = GetCurrentHole();

                if (currentHole == 1)
                {
                    return _allHoles.Single(h => h.Id == 1).Par;
                }
                else
                {
                    int totalPoints = _allHoles.Where(h => h.Id <= currentHole).Sum(h => h.Par);
                    int totalPointsTaken = Game.Shots.Where(s => s.Hole.Id < currentHole).Sum(s => s.Points);
                    int pointsAvailable = totalPoints - totalPointsTaken;

                    return pointsAvailable;
                }
            }
        }

        public IEnumerable<Player> Leaderboard
        {
            get
            {
                return ActivePlayers.OrderBy(p => p.Shots.Where(s => s.Game.Id == Game.Id).Sum(s => s.Attempts));
            }
        }

        private IEnumerable<Player> _allPlayers;
        public string Player;

        public IEnumerable<SelectListItem> AllPlayers
        {
            get
            {
                var currentPlayer = GetCurrentPlayer();

                return _allPlayers.Select(player => new SelectListItem() { Text = player.Name, Value = player.Id.ToString(), Selected = (player.Id == currentPlayer.Id) }).ToList();
            }
        }

        private IEnumerable<Hole> _allHoles;
        public string Hole;

        public IEnumerable<SelectListItem> AllHoles
        {
            get
            {
                int currentHole = GetCurrentHole();

                return _allHoles.Select(hole => new SelectListItem() { Text = hole.Id.ToString(), Value = hole.Id.ToString(), Selected = (hole.Id == currentHole) }).ToList();
            }
        }

        private IEnumerable<ShotType> _scoreTypes;
        public string ScoreType;

        public IEnumerable<SelectListItem> ScoreTypes
        {
            get
            {
                return _scoreTypes.Select(scoreType => new SelectListItem() { Text = scoreType.Name, Value = scoreType.Id.ToString() }).ToList();
            }
        }
    }
}