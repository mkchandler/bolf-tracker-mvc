using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHoleRepository _holeRepository;
        private readonly IShotRepository _shotRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerStatisticsRepository _playerStatisticsRepository;
        private readonly IPlayerHoleStatisticsRepository _playerHoleStatisticsRepository;
        private readonly IGameStatisticsRepository _gameStatisticsRepository;
        private readonly IPlayerGameStatisticsRepository _playerGameStatisticsRepository;
        private readonly IPlayerCareerStatisticsRepository _playerCareerStatisticsRepository;
        private readonly IPlayerBadgesRepository _playerBadgeRepository;

        // TODO: Really need to figure out a better way to do this
        private const int ShotTypeMake = 1;
        private const int ShotTypeMiss = 2;
        private const int ShotTypePush = 3;
        private const int ShotTypeSteal = 4;
        private const int ShotTypeSugarFreeSteal = 5;

        public PlayerService(IPlayerRepository playerRepository, IHoleRepository holeRepository, IShotRepository shotRepository, IGameRepository gameRepository, IPlayerStatisticsRepository playerStatisticsRepository, IPlayerHoleStatisticsRepository playerHoleStatisticsRepository, IGameStatisticsRepository gameStatisticsRepository, IPlayerGameStatisticsRepository playerGameStatisticsRepository, IPlayerCareerStatisticsRepository playerCareerStatisticsRepository, IPlayerBadgesRepository playerBadgeRepository)
        {
            _playerRepository = playerRepository;
            _holeRepository = holeRepository;
            _shotRepository = shotRepository;
            _gameRepository = gameRepository;
            _playerStatisticsRepository = playerStatisticsRepository;
            _playerHoleStatisticsRepository = playerHoleStatisticsRepository;
            _gameStatisticsRepository = gameStatisticsRepository;
            _playerGameStatisticsRepository = playerGameStatisticsRepository;
            _playerCareerStatisticsRepository = playerCareerStatisticsRepository;
            _playerBadgeRepository = playerBadgeRepository;
        }

        public Player GetPlayer(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return _playerRepository.GetById(id);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _playerRepository.All().ToList();
        }

        public Player Create(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name", "Player name must contain a value");

            var player = new Player() { Name = name };

            _playerRepository.Add(player);

            return player;
        }

        public Player Update(int id, string name)
        {
            var player = _playerRepository.GetById(id);
            player.Name = name;

            _playerRepository.Update(player);

            return player;
        }

        public void Delete(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            _playerRepository.Delete(_playerRepository.GetById(id));
        }

        public IEnumerable<PlayerBadges> GetPlayerBadges(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");
            return _playerBadgeRepository.GetByPlayer(id);
        }

        public void CalculatePlayerStatistics()
        {
            var months = _gameRepository.GetActiveMonthsAndYears();

            foreach (var month in months)
            {
                CalculatePlayerStatistics(month.Item1, month.Item2, false);
                CalculatePlayerHoleStatistics(month.Item1, month.Item2);
            }

            DeletePlayerCareerStatistics();
            var players = _playerRepository.All().ToList();

            foreach (var player in players)
            {
                CalculatePlayerCareerStatistics(player);
            }
        }

        public void CalculatePlayerStatistics(int month, int year, bool calculateCareerStatistics)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            DeletePlayerStatistics(month, year);
            if (calculateCareerStatistics) DeletePlayerCareerStatistics();

            var players = _playerRepository.GetActiveByMonthAndYear(month, year);
            var allPlayersGameStatistics = _playerGameStatisticsRepository.GetByMonthAndYear(month, year);

            foreach (var player in players)
            {
                var playerGameStatistics = allPlayersGameStatistics.Where(pgs => pgs.Player.Id == player.Id);

                var playerStatistics = CalculatePlayerStatistics(player, playerGameStatistics, month, year);
                _playerStatisticsRepository.Add(playerStatistics);

                if (calculateCareerStatistics)
                {
                    CalculatePlayerCareerStatistics(player);
                }
            }

            CalculatePlayerHoleStatistics(month, year);
        }

        private void CalculatePlayerCareerStatistics(Player player)
        {
            var playerCareerGameStatistics = _playerGameStatisticsRepository.GetByPlayer(player.Id);

            var playerCareerStatistics = CalculatePlayerCareerStatistics(player, playerCareerGameStatistics);
            _playerCareerStatisticsRepository.Add(playerCareerStatistics);
        }

        public void CalculatePlayerHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            DeletePlayerHoleStatistics(month, year);

            var players = _playerRepository.All().ToList();
            var shots = _shotRepository.GetByMonthAndYear(month, year);
            var holes = _holeRepository.All().ToList();

            foreach (var player in players)
            {
                var playerShots = shots.Where(s => s.Player.Id == player.Id);

                if (playerShots.Any())
                {
                    foreach (var hole in holes)
                    {
                        var playerHoleShots = playerShots.Where(s => s.Hole.Id == hole.Id);

                        if (playerHoleShots.Any())
                        {
                            var playerHoleStatistics = new PlayerHoleStatistics() { Player = player, Hole = hole, Month = month, Year = year };

                            playerHoleStatistics.ShotsMade = playerHoleShots.Count(s => s.ShotMade);
                            playerHoleStatistics.Attempts = playerHoleShots.Sum(s => s.Attempts);
                            playerHoleStatistics.ShootingPercentage = Decimal.Round((decimal)playerHoleStatistics.ShotsMade / (decimal)playerHoleStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
                            playerHoleStatistics.PointsScored = playerHoleShots.Where(s => s.ShotType.Id != ShotTypePush).Sum(s => s.Points);
                            playerHoleStatistics.Pushes = playerHoleShots.Count(s => s.ShotType.Id == ShotTypePush);
                            playerHoleStatistics.Steals = playerHoleShots.Count(s => s.ShotType.Id == ShotTypeSteal);
                            playerHoleStatistics.SugarFreeSteals = playerHoleShots.Count(s => s.ShotType.Id == ShotTypeSugarFreeSteal);

                            _playerHoleStatisticsRepository.Add(playerHoleStatistics);
                        }
                    }
                }
            }
        }

        public PlayerStatistics GetPlayerStatistics(int playerId, int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerStatisticsRepository.GetByPlayerMonthAndYear(playerId, month, year);
        }

        public IEnumerable<PlayerStatistics> GetPlayerStatistics(int playerId)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");

            return _playerStatisticsRepository.GetByPlayer(playerId);
        }

        public IEnumerable<PlayerStatistics> GetPlayerStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerStatisticsRepository.GetByMonthAndYear(month, year);
        }

        public IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");

            return _playerGameStatisticsRepository.GetByPlayer(playerId);
        }

        public IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerGameStatisticsRepository.GetByMonthAndYear(month, year);
        }

        public IEnumerable<PlayerGameStatistics> GetPlayerGameStatistics(int playerId, int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerGameStatisticsRepository.GetByPlayerMonthAndYear(playerId, month, year);
        }

        public PlayerCareerStatistics GetPlayerCareerStatistics(int playerId)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");

            return _playerCareerStatisticsRepository.GetByPlayer(playerId);
        }

        public IEnumerable<PlayerCareerStatistics> GetPlayerCareerStatistics()
        {
            return _playerCareerStatisticsRepository.All();
        }

        public IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerHoleStatisticsRepository.GetByMonthAndYear(month, year);
        }

        public IEnumerable<PlayerHoleStatistics> GetPlayerHoleStatistics(int playerId, int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerHoleStatisticsRepository.GetByPlayerMonthAndYear(playerId, month, year);
        }

        private PlayerStatistics CalculatePlayerStatistics(Player player, IEnumerable<PlayerGameStatistics> playerGameStatistics, int month, int year)
        {
            var playerStatistics = new PlayerStatistics() { Player = player, Month = month, Year = year };

            playerStatistics.Wins = playerGameStatistics.Count(gs => gs.Winner);
            playerStatistics.Losses = playerGameStatistics.Count(gs => !gs.Winner);
            playerStatistics.WinningPercentage = Decimal.Round((decimal)playerStatistics.Wins / (decimal)playerStatistics.TotalGames, 3, MidpointRounding.AwayFromZero);
            playerStatistics.ShotsMade = playerGameStatistics.Sum(gs => gs.ShotsMade);
            playerStatistics.Attempts = playerGameStatistics.Sum(gs => gs.Attempts);
            playerStatistics.ShootingPercentage = Decimal.Round((decimal)playerStatistics.ShotsMade / (decimal)playerStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
            playerStatistics.Points = playerGameStatistics.Sum(gs => gs.Points);
            playerStatistics.PointsPerGame = Decimal.Round((decimal)playerStatistics.Points / (decimal)playerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerStatistics.Pushes = playerGameStatistics.Sum(gs => gs.Pushes);
            playerStatistics.PushesPerGame = Decimal.Round((decimal)playerStatistics.Pushes / (decimal)playerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerStatistics.Steals = playerGameStatistics.Sum(gs => gs.Steals);
            playerStatistics.StealsPerGame = Decimal.Round((decimal)playerStatistics.Steals / (decimal)playerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero); ;
            playerStatistics.SugarFreeSteals = playerGameStatistics.Sum(gs => gs.SugarFreeSteals);
            playerStatistics.SugarFreeStealsPerGame = Decimal.Round((decimal)playerStatistics.SugarFreeSteals / (decimal)playerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);

            return playerStatistics;
        }

        private PlayerCareerStatistics CalculatePlayerCareerStatistics(Player player, IEnumerable<PlayerGameStatistics> playerGameStatistics)
        {
            var playerCareerStatistics = new PlayerCareerStatistics() { Player = player };

            playerCareerStatistics.Wins = playerGameStatistics.Count(pgs => pgs.Winner);
            playerCareerStatistics.Losses = playerGameStatistics.Count(pgs => !pgs.Winner);
            playerCareerStatistics.WinningPercentage = Decimal.Round((decimal)playerCareerStatistics.Wins / (decimal)playerCareerStatistics.TotalGames, 3, MidpointRounding.AwayFromZero);
            playerCareerStatistics.ShotsMade = playerGameStatistics.Sum(pgs => pgs.ShotsMade);
            playerCareerStatistics.Attempts = playerGameStatistics.Sum(pgs => pgs.Attempts);
            playerCareerStatistics.ShotsMissed = playerCareerStatistics.Attempts - playerCareerStatistics.ShotsMade;
            playerCareerStatistics.ShootingPercentage = Decimal.Round((decimal)playerCareerStatistics.ShotsMade / (decimal)playerCareerStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
            playerCareerStatistics.Points = playerGameStatistics.Sum(pgs => pgs.Points);
            playerCareerStatistics.PointsPerGame = Decimal.Round((decimal)playerCareerStatistics.Points / (decimal)playerCareerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerCareerStatistics.Pushes = playerGameStatistics.Sum(pgs => pgs.Pushes);
            playerCareerStatistics.PushesPerGame = Decimal.Round((decimal)playerCareerStatistics.Pushes / (decimal)playerCareerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerCareerStatistics.Steals = playerGameStatistics.Sum(pgs => pgs.Steals);
            playerCareerStatistics.StealsPerGame = Decimal.Round((decimal)playerCareerStatistics.Steals / (decimal)playerCareerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero); ;
            playerCareerStatistics.SugarFreeSteals = playerGameStatistics.Sum(pgs => pgs.SugarFreeSteals);
            playerCareerStatistics.SugarFreeStealsPerGame = Decimal.Round((decimal)playerCareerStatistics.SugarFreeSteals / (decimal)playerCareerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerCareerStatistics.StainlessSteals = playerGameStatistics.Sum(pgs => pgs.StainlessSteals);
            playerCareerStatistics.StainlessStealsPerGame = Decimal.Round((decimal)playerCareerStatistics.StainlessSteals / (decimal)playerCareerStatistics.TotalGames, 1, MidpointRounding.AwayFromZero);
            playerCareerStatistics.GameWinningSteals = playerGameStatistics.Count(pgs => pgs.GameWinningSteal);
            playerCareerStatistics.OvertimeWins = playerGameStatistics.Count(pgs => pgs.OvertimeWin);
            playerCareerStatistics.RegulationWins = playerGameStatistics.Count(pgs => !pgs.OvertimeWin && pgs.Winner);
            playerCareerStatistics.Shutouts = playerGameStatistics.Count(pgs => pgs.Shutout);
            playerCareerStatistics.PerfectGames = playerGameStatistics.Count(pgs => pgs.PerfectGame);

            return playerCareerStatistics;
        }

        private void DeletePlayerStatistics(int month, int year)
        {
            _playerStatisticsRepository.DeleteByMonthAndYear(month, year);
        }

        private void DeletePlayerCareerStatistics()
        {
            _playerCareerStatisticsRepository.DeleteAll();
        }

        private void DeletePlayerHoleStatistics(int month, int year)
        {
            _playerHoleStatisticsRepository.DeleteByMonthAndYear(month, year);
        }
    }
}
