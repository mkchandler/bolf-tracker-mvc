using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Infrastructure;
using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerStatisticsRepository _playerStatisticsRepository;
        private readonly IGameStatisticsRepository _gameStatisticsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IPlayerRepository playerRepository, IPlayerStatisticsRepository playerStatisticsRepository, IGameStatisticsRepository gameStatisticsRepository, IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _playerStatisticsRepository = playerStatisticsRepository;
            _gameStatisticsRepository = gameStatisticsRepository;
            _unitOfWork = unitOfWork;
        }

        public Player GetPlayer(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return _playerRepository.GetById(id);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _playerRepository.All();
        }

        public Player Create(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name", "Player name must contain a value");

            var player = new Player() { Name = name };

            _playerRepository.Add(player);
            _unitOfWork.Commit();

            return player;
        }

        public Player Update(int id, string name)
        {
            var player = _playerRepository.GetById(id);

            player.Name = name;

            _unitOfWork.Commit();

            return player;
        }

        public void Delete(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            _playerRepository.Delete(_playerRepository.GetById(id));

            _unitOfWork.Commit();
        }

        public void CalculatePlayerStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            var players = _playerRepository.All();

            foreach (var player in players)
            {
                var playerStatistics = CalculatePlayerStatistics(player, month, year);

                _playerStatisticsRepository.Add(playerStatistics);
            }

            _unitOfWork.Commit();
        }

        public void CalculatePlayerStatistics(int playerId, int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            var player = _playerRepository.GetById(playerId);

            var playerStatistics = CalculatePlayerStatistics(player, month, year);

            _playerStatisticsRepository.Add(playerStatistics);
            _unitOfWork.Commit();
        }

        public PlayerStatistics GetPlayerStatistics(int playerId, int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerStatisticsRepository.GetByPlayerAndMonth(playerId, month, year);
        }

        public IEnumerable<PlayerStatistics> GetPlayersStatistics(int playerId)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");

            return _playerStatisticsRepository.GetByPlayer(playerId);
        }

        public IEnumerable<PlayerStatistics> GetPlayersStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _playerStatisticsRepository.GetByMonthAndYear(month, year);
        }

        private PlayerStatistics CalculatePlayerStatistics(Player player, int month, int year)
        {
            var playerStatistics = new PlayerStatistics() { Player = player, Month = month, Year = year };

            var playerGameStatistics = _gameStatisticsRepository.GetByPlayerAndMonth(player.Id, month, year);

            playerStatistics.ShotsMade = playerGameStatistics.Sum(gs => gs.ShotsMade);
            playerStatistics.Attempts = playerGameStatistics.Sum(gs => gs.Attempts);
            playerStatistics.Points = playerGameStatistics.Sum(gs => gs.Points);
            playerStatistics.Pushes = playerGameStatistics.Sum(gs => gs.Pushes);
            playerStatistics.Steals = playerGameStatistics.Sum(gs => gs.Steals);
            playerStatistics.SugarFreeSteals = playerGameStatistics.Sum(gs => gs.SugarFreeSteals);
            playerStatistics.Wins = playerGameStatistics.Count(gs => gs.Winner);
            playerStatistics.Losses = playerGameStatistics.Count(gs => !gs.Winner);

            return playerStatistics;
        }
    }
}