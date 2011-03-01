using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BolfTracker.Models;
using System;

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
                return Game.Shots.Select(s => s.Player).Distinct();
            }
        }

        private Player GetCurrentPlayer()
        {
            var playerResult = new Player();

            if (Game.Shots.Any())
            {
                var activePlayers = ActivePlayers;
                var playersDescending = Game.Shots.OrderByDescending(s => s.Id).Take(activePlayers.Count()).Select(s => s.Player);
                var duplicatePlayers = Game.Shots.GroupBy(s => s.Player.Id).Where(p => p.Count() > 1);

                // Check to see if we've had any duplicate players yet (if so, that means we can determine the order)
                if (duplicatePlayers.Any())
                {
                    // If we are on the last hole or in overtime, the order could change because not
                    // everyone can win
                    int currentHole = GetCurrentHole();

                    if (currentHole >= 10)
                    {
                        int currentPointsAvailable = PointsAvailable;
                        int maxPointsAtCurrentHole = _allHoles.Where(h => h.Id <= currentHole).Sum(h => h.Par);
                        int totalPointsTaken = maxPointsAtCurrentHole - currentPointsAvailable;
                        var leaderboard = Leaderboard;
                        var leaderPoints = leaderboard.Max(l => l.Points);

                        var playersWhoCanWin = new List<LeaderboardViewModel>();

                        foreach (var player in leaderboard)
                        {
                            if ((player.Points + currentPointsAvailable) > leaderPoints)
                            {
                                if (!player.Player.Shots.Any(s => s.Game.Id == Game.Id && s.Hole.Id == currentHole))
                                {
                                    playersWhoCanWin.Add(player);
                                }
                            }
                        }

                        if (!playersWhoCanWin.Any())
                        {
                            return playersDescending.Last();
                        }

                        return playersWhoCanWin.OrderByDescending(l => l.Points).First().Player;
                    }

                    return playersDescending.Last();
                }
                else
                {
                    // If we can't determine the order, just get the next player who has not gone already
                    foreach (var player in _allPlayers)
                    {
                        if (!activePlayers.Contains(player))
                        {
                            return player;
                        }
                    }
                }
            }

            return playerResult;
        }

        private IEnumerable<Player> PlayersWhoCanWin()
        {
            throw new NotImplementedException();
        }

        public int GetCurrentHole()
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

        public IEnumerable<LeaderboardViewModel> Leaderboard
        {
            get
            {
                return ActivePlayers.Select(p => new LeaderboardViewModel(p, Game.Id)).OrderByDescending(l => l.Points);
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

        public IEnumerable<Hole> Holes
        {
            get
            {
                return _allHoles;
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

    public class LeaderboardViewModel
    {
        public LeaderboardViewModel(Player player, int gameId)
        {
            Player = player;
            Points = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Points);
            ShotsMade = player.Shots.Count(s => s.Game.Id == gameId && s.ShotMade);
            Attempts = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Attempts);
            ShootingPercentage = Decimal.Round(Convert.ToDecimal(ShotsMade) / Convert.ToDecimal(Attempts), 2, MidpointRounding.AwayFromZero);
            Steals = player.Shots.Count(s => s.Game.Id == gameId && (s.ShotType.Id == 4 || s.ShotType.Id == 5));
            Pushes = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == 3);
        }

        public Player Player
        {
            get;
            private set;
        }

        public int Points
        {
            get;
            private set;
        }

        public int ShotsMade
        {
            get;
            private set;
        }

        public int Attempts
        {
            get;
            private set;
        }

        public decimal ShootingPercentage
        {
            get;
            private set;
        }

        public int Steals
        {
            get;
            private set;
        }

        public int Pushes
        {
            get;
            private set;
        }
    }
}