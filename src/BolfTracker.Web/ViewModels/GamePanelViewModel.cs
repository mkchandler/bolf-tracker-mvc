using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    // WARNING: This class is a mess right now and needs to be cleaned up.  Beware all ye who enter.  I warned you.
    public class GamePanelViewModel
    {
        private int? _currentHole;
        private int? _pointsAvailable;
        private bool? _gameFinalized;

        public GamePanelViewModel(Game game, IEnumerable<Player> allPlayers, IEnumerable<Hole> allHoles)
        {
            Game = game;

            _allPlayers = allPlayers.ToList();
            _allHoles = allHoles.ToList();
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
                if (_gameFinalized.HasValue)
                {
                    return _gameFinalized.Value;
                }
                else
                {
                    _gameFinalized = Game.GameStatistics.Any();

                    return _gameFinalized.Value;
                }
            }
        }

        private IEnumerable<Player> _activePlayers = null;

        public IEnumerable<Player> ActivePlayers
        {
            get
            {
                return _activePlayers ?? (_activePlayers = Game.Shots.Select(s => s.Player).Distinct());
            }
        }

        private Player GetCurrentPlayer()
        {
            var playerResult = new Player();
            int currentHole = GetCurrentHole();

            if (Game.Shots.Any())
            {
                var activePlayers = ActivePlayers;
                var playersDescending = GetCurrentActivePlayers(activePlayers, includeOvertime: false);

                var duplicatePlayers = Game.Shots.GroupBy(s => s.Player.Id).Where(p => p.Count() > 1);

                // Check to see if we've had any duplicate players yet (if so, that means we can determine the order)
                if (duplicatePlayers.Any())
                {
                    // If we are on the last hole or in overtime, the order could change because not everyone can win
                    if (currentHole >= 10)
                    {
                        var playersWhoCanWin = GetPlayersWhoCanWin(currentHole);

                        if (!playersWhoCanWin.Any() || playersWhoCanWin.Count() >= playersDescending.Count())
                        {
                            // If all of the players can win, we will go in normal order
                            var lastPlayerToShoot = Game.Shots.Where(s => s.Game.Id == Game.Id).OrderByDescending(s => s.Id).Select(s => s.Player).First();

                            var playerList = playersDescending.Reverse().ToList();

                            int index = playerList.IndexOf(lastPlayerToShoot);

                            return playerList[index == -1 ? 0 : (index + 1) % (playerList.Count)];
                        }
                        else
                        {
                            // If only some of the players can win, we will go in descending order by points
                            var playersWhoCanStillWin = new List<LeaderboardViewModel>();

                            foreach (var player in playersWhoCanWin)
                            {
                                var playerCurrentHoleShots = Game.Shots.Where(s => s.Player.Id == player.Player.Id && s.Game.Id == Game.Id && s.Hole.Id == currentHole);

                                if (!playerCurrentHoleShots.Any())
                                {
                                    playersWhoCanStillWin.Add(player);
                                }
                            }

                            var playersWhoCannotWin = new List<Player>();

                            if (playersWhoCanStillWin.Count == 0)
                            {
                                // This means that one of the players who could win made a shot, so all of the people 
                                // that cannot win need to take a shot to push them
                                foreach (var player in playersDescending)
                                {
                                    if (!Game.Shots.Any(s => s.Player.Id == player.Id && s.Hole.Id == currentHole))
                                    {
                                        if (!playersWhoCanWin.Any(l => l.Player.Id == player.Id))
                                        {
                                            playersWhoCannotWin.Add(player);
                                        }
                                    }
                                }

                                // TODO: Here we need to figure out the order that these players who can push
                                // the hole need to go in.  My thought is that the players with the least amount of
                                // pushes for the current period (in our case month) get to go first.
                                return playersWhoCannotWin.Last();
                            }
                            else
                            {
                                return playersWhoCanStillWin.OrderByDescending(l => l.Points).First().Player;
                            }
                        }
                    }

                    return playersDescending.Last();
                }
                else
                {
                    // TODO: My thought for initial player order is that the people with the lowest shooting percentage
                    // get to go first, and once one of them makes it, the players with the lowest pushes get to go second

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

        private IEnumerable<Player> _currentActivePlayers = null;

        private IEnumerable<Player> GetCurrentActivePlayers(IEnumerable<Player> gamePlayers, bool includeOvertime)
        {
            if (_currentActivePlayers == null)
            {
                var currentActivePlayers = new List<Player>();

                Func<Shot, bool> query;

                if (includeOvertime)
                {
                    query = s => s.Game.Id == Game.Id;
                }
                else
                {
                    query = s => s.Game.Id == Game.Id && s.Hole.Id < 10;
                }

                // TODO: The part where we check for holes less than 10 will need to change when we implement the new hole logic
                foreach (var shot in Game.Shots.Where(query).OrderByDescending(s => s.Id))
                {
                    if (!currentActivePlayers.Any(p => p.Id == shot.Player.Id))
                    {
                        currentActivePlayers.Add(shot.Player);
                    }
                    else
                    {
                        break;
                    }
                }

                _currentActivePlayers = currentActivePlayers;
            }

            return _currentActivePlayers;
        }

        private IEnumerable<LeaderboardViewModel> _playersWhoCanWin = null;

        private IEnumerable<LeaderboardViewModel> GetPlayersWhoCanWin(int currentHole)
        {
            if (_playersWhoCanWin == null)
            {
                int currentPointsAvailable = GetPointsAvailable(currentHole);
                var leaderboard = Leaderboard.OrderByDescending(l => l.Points);

                var leader = leaderboard.First();

                // This is the leader's points not counting any temporary points scored on the current hole
                var leaderPoints = Game.Shots.Where(s => s.Player.Id == leader.Player.Id && s.Hole.Id < currentHole).Sum(s => s.Points);

                var playersWhoCanWin = new List<LeaderboardViewModel>();

                foreach (var player in leaderboard)
                {
                    // If the player has already gone on this hole and made the shot then we need to 
                    // subtract those points for the next calculation
                    var newPlayerCurrentHoleShot = Game.Shots.Where(s => s.Player.Id == player.Player.Id && s.Hole.Id == currentHole && s.Points > 0);

                    int playerCurrentHolePoints = newPlayerCurrentHoleShot.Any() ? newPlayerCurrentHoleShot.First().Points : 0;

                    // If the player can at least tie the leader, then he gets to take all shots
                    if (((player.Points - playerCurrentHolePoints) + currentPointsAvailable) >= leaderPoints)
                    {
                        playersWhoCanWin.Add(player);
                    }
                }

                _playersWhoCanWin = playersWhoCanWin;
            }

            return _playersWhoCanWin;
        }

        public int GetCurrentHole()
        {
            if (_currentHole.HasValue)
            {
                return _currentHole.Value;
            }
            else
            {
                _currentHole = 1;

                if (Game.Shots.Any())
                {
                    _currentHole = Game.Shots.Max(s => s.Hole.Id);
                    var holeShots = Game.Shots.Where(s => s.Hole.Id == _currentHole.Value).ToList();

                    if (_currentHole.Value == 1)
                    {
                        // If we are on the first hole, there's no way of knowing when to go to the next hole
                        // because we don't know how many players are going (unless it's been pushed on 1)
                        if (holeShots.Count(s => s.Attempts == 1 && s.ShotMade == true) > 1)
                        {
                            _currentHole += 1;
                            return _currentHole.Value;
                        }
                        else
                        {
                            return _currentHole.Value;
                        }
                    }
                    else
                    {
                        // If the hole was pushed on 1, go to the next hole
                        if (holeShots.Count(s => s.Attempts == 1 && s.ShotMade == true) > 1)
                        {
                            _currentHole += 1;
                            return _currentHole.Value;
                        }

                        var totalPlayers = GetCurrentActivePlayers(ActivePlayers, includeOvertime: false).Count();

                        // If everyone has gone, go to the next hole
                        if (holeShots.Count() == totalPlayers)
                        {
                            _currentHole++;
                        }

                        // TODO: This needs to change to a MAX function for hole number when the new hole/overtime logic is added
                        if (_currentHole.Value >= 10)
                        {
                            // NOTE: Anything going off of x.Player.Shots is runs a shitload of queries... avoid
                            var playersWhoCanWin = GetPlayersWhoCanWin(_currentHole.Value).Where(g => Game.Shots.Count(s => s.Hole.Id == _currentHole.Value && !s.ShotMade) == 0);

                            if (playersWhoCanWin.Count() == 0)
                            {
                                _currentHole++;
                                return _currentHole.Value;
                            }
                            else
                            {
                                return _currentHole.Value;
                            }
                        }
                    }
                }

                return _currentHole.Value;
            }
        }

        public int PointsAvailable
        {
            get { return GetPointsAvailable(GetCurrentHole()); }
        }

        public int GetPointsAvailable(int currentHole)
        {
            if (_pointsAvailable.HasValue)
            {
                return _pointsAvailable.Value;
            }
            else
            {
                if (currentHole == 1)
                {
                    _pointsAvailable = _allHoles.Single(h => h.Id == 1).Par;

                    return _pointsAvailable.Value;
                }
                else
                {
                    int totalPoints = _allHoles.Where(h => h.Id <= currentHole).Sum(h => h.Par);
                    int totalPointsTaken = Game.Shots.Where(s => s.Hole.Id < currentHole).Sum(s => s.Points);

                    _pointsAvailable = totalPoints - totalPointsTaken;

                    return _pointsAvailable.Value;
                }
            }
        }

        private IEnumerable<LeaderboardViewModel> _leaderboard = null;

        public IEnumerable<LeaderboardViewModel> Leaderboard
        {
            get
            {
                if (_leaderboard == null)
                {
                    var leaderboard = new List<LeaderboardViewModel>();

                    foreach (var player in ActivePlayers)
                    {
                        var playerShots = Game.Shots.Where(s => s.Player.Id == player.Id).ToList();

                        leaderboard.Add(new LeaderboardViewModel(player, playerShots, Game));
                    }

                    _leaderboard = leaderboard.OrderByDescending(l => l.Points);

                    return _leaderboard;
                }
                else
                {
                    return _leaderboard;
                }
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
    }

    public class LeaderboardViewModel
    {
        public LeaderboardViewModel(Player player, IEnumerable<Shot> playerShots, Game game)
        {
            Player = player;
            Points = playerShots.Sum(s => s.Points);
            ShotsMade = playerShots.Count(s => s.ShotMade);
            Attempts = playerShots.Sum(s => s.Attempts);
            ShootingPercentage = Decimal.Round(Convert.ToDecimal(ShotsMade) / Convert.ToDecimal(Attempts), 2, MidpointRounding.AwayFromZero);
            Steals = game.Shots.Count(s => s.Player.Id == player.Id && (s.ShotType.Id == 4 || s.ShotType.Id == 5));
            Pushes = game.Shots.Count(s => s.Player.Id == player.Id && s.ShotType.Id == 3);
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