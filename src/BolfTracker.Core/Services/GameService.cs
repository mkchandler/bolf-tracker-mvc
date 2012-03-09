using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Infrastructure;
using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IShotRepository _shotRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameStatisticsRepository _gameStatisticsRepository;
        private readonly IPlayerGameStatisticsRepository _playerGameStatisticsRepository;

        // TODO: Really need to figure out a better way to do this
        private const int ShotTypePush = 3;
        private const int ShotTypeSteal = 4;
        private const int ShotTypeSugarFreeSteal = 5;

        public GameService(IGameRepository gameRepository, IShotRepository shotRepository, IPlayerRepository playerRepository, IGameStatisticsRepository gameStatisticsRepository, IPlayerGameStatisticsRepository playerGameStatisticsRepository)
        {
            _gameRepository = gameRepository;
            _shotRepository = shotRepository;
            _playerRepository = playerRepository;
            _gameStatisticsRepository = gameStatisticsRepository;
            _playerGameStatisticsRepository = playerGameStatisticsRepository;
        }

        public Game GetGame(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return _gameRepository.GetById(id);
        }

        public IEnumerable<Game> GetGames(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _gameRepository.GetByMonthAndYear(month, year);
        }

        public Game CreateGame(DateTime date)
        {
            var game = new Game() { Date = date };

            _gameRepository.Add(game);

            return game;
        }

        public Game UpdateGame(int id, DateTime date)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            throw new NotImplementedException();
        }

        public void DeleteGame(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            throw new NotImplementedException();
        }

        public void CalculateGameStatistics()
        {
            var games = _gameRepository.All().ToList();

            if (games.Any())
            {
                DeleteGameStatistics();

                foreach (var game in games)
                {
                    CalculateGameStatistics(game.Id);
                }
            }
        }

        public void CalculateGameStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            var games = _gameRepository.GetFinalizedByMonthAndYear(month, year);

            if (games.Any())
            {
                DeleteGameStatistics(month, year);

                foreach (var game in games)
                {
                    CalculateGameStatistics(game.Id);
                }
            }
        }

        public void CalculateGameStatistics(int gameId)
        {
            Check.Argument.IsNotZeroOrNegative(gameId, "gameId");

            var game = _gameRepository.GetById(gameId);
            var gameShots = _shotRepository.GetByGame(gameId);
            var players = _playerRepository.GetByGame(gameId);

            int maxPoints = 0;

            // The first thing that we want to calculate is the max number of points scored (the winner's points). This way 
            // we can easily determine who the winner is later when actually adding the stats to the repository.
            foreach (var player in players)
            {
                int playerPoints = gameShots.Where(s => s.Player.Id == player.Id).Sum(s => s.Points);

                if (playerPoints > maxPoints)
                {
                    maxPoints = playerPoints;
                }
            }

            // Calculate all of the game statistics for each player individually
            foreach (var player in players)
            {
                var playerGameStatistics = new PlayerGameStatistics();

                var playerShots = gameShots.Where(s => s.Player.Id == player.Id);

                playerGameStatistics.Game = game;
                playerGameStatistics.Player = player;
                playerGameStatistics.Points = playerShots.Sum(s => s.Points);
                playerGameStatistics.ShotsMade = playerShots.Count(s => s.ShotMade);
                playerGameStatistics.Attempts = playerShots.Sum(s => s.Attempts);
                playerGameStatistics.ShootingPercentage = Decimal.Round((decimal)playerGameStatistics.ShotsMade / (decimal)playerGameStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
                playerGameStatistics.Pushes = playerShots.Count(s => s.ShotType.Id == ShotTypePush);
                playerGameStatistics.Steals = playerShots.Count(s => s.ShotType.Id == ShotTypeSteal);
                playerGameStatistics.SugarFreeSteals = playerShots.Count(s => s.ShotType.Id == ShotTypeSugarFreeSteal);
                playerGameStatistics.Winner = (playerGameStatistics.Points == maxPoints);
                playerGameStatistics.OvertimeWin = playerShots.Max(s => s.Hole.Id > 10 && playerGameStatistics.Winner);

                int totalGamePoints = gameShots.Sum(s => s.Points);

                playerGameStatistics.Shutout = playerGameStatistics.Points == totalGamePoints;
                playerGameStatistics.PerfectGame = playerGameStatistics.Shutout && (playerGameStatistics.ShotsMade == playerGameStatistics.Attempts);

                _playerGameStatisticsRepository.Add(playerGameStatistics);
            }

            // Calculate the total stats for the game
            var gameStatistics = new GameStatistics();

            gameStatistics.Game = game;
            gameStatistics.HoleCount = gameShots.Select(s => s.Hole.Id).Distinct().Count();

            // if HoleCount mod NumberOfHoles = 0 --> OvertimeCount = HoleCount / NumberOfHoles
            // else OvertimeCount = (HoleCount - (HoleCount mod NumberOfHoles)) / NumberOfHoles

            gameStatistics.OvertimeCount = (gameShots.Max(s => s.Hole.Id) > 10) ? 1 : 0;
            gameStatistics.PlayerCount = gameShots.Select(s => s.Player.Id).Distinct().Count();
            gameStatistics.Points = gameShots.Sum(s => s.Points);
            gameStatistics.ShotsMade = gameShots.Count(s => s.ShotMade);
            gameStatistics.Attempts = gameShots.Sum(s => s.Attempts);
            gameStatistics.ShotsMissed = gameStatistics.Attempts - gameStatistics.ShotsMade;
            gameStatistics.ShootingPercentage = Decimal.Round((decimal)gameStatistics.ShotsMade / (decimal)gameStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
            gameStatistics.Pushes = gameShots.Count(s => s.ShotType.Id == ShotTypePush);
            gameStatistics.Steals = gameShots.Count(s => s.ShotType.Id == ShotTypeSteal);
            gameStatistics.SugarFreeSteals = gameShots.Count(s => s.ShotType.Id == ShotTypeSugarFreeSteal);
            gameStatistics.StainlessSteals = gameShots.Count(s => (s.ShotType.Id == ShotTypeSteal || s.ShotType.Id == ShotTypeSugarFreeSteal) && s.ShotMade && s.Attempts == 1);

            _gameStatisticsRepository.Add(gameStatistics);
        }

        private void DeleteGameStatistics()
        {
            _gameStatisticsRepository.DeleteAll();
            _playerGameStatisticsRepository.DeleteAll();
        }

        private void DeleteGameStatistics(int month, int year)
        {
            _gameStatisticsRepository.DeleteByMonthAndYear(month, year);
            _playerGameStatisticsRepository.DeleteByMonthAndYear(month, year);
        }
    }
}
