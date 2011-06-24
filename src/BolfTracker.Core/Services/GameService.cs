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
        private readonly IGameStatisticsRepository _gameStatisticsRepository;
        private readonly IPlayerGameStatisticsRepository _playerGameStatisticsRepository;
        private readonly IUnitOfWork _unitOfWork;

        // TODO: Really need to figure out a better way to do this
        private const int ShotTypePush = 3;
        private const int ShotTypeSteal = 4;
        private const int ShotTypeSugarFreeSteal = 5;

        public GameService(IGameRepository gameRepository, IGameStatisticsRepository gameStatisticsRepository, IPlayerGameStatisticsRepository playerGameStatisticsRepository, IUnitOfWork unitOfWork)
        {
            _gameRepository = gameRepository;
            _gameStatisticsRepository = gameStatisticsRepository;
            _playerGameStatisticsRepository = playerGameStatisticsRepository;
            _unitOfWork = unitOfWork;
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
            _unitOfWork.Commit();

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
            var games = _gameRepository.All();

            foreach (var game in games)
            {
                var players = game.Shots.Select(s => s.Player).Distinct();

                int maxPoints = 0;

                // The first thing that we want to calculate is the max number of points scored (the winner's points). This way 
                // we can easily determine who the winner is later when actually adding the stats to the repository.
                foreach (var player in players)
                {
                    int playerPoints = player.Shots.Where(s => s.Game.Id == game.Id).Sum(s => s.Points);

                    if (playerPoints > maxPoints)
                    {
                        maxPoints = playerPoints;
                    }
                }

                // Calculate all of the game statistics for each player individually
                foreach (var player in players)
                {
                    var playerGameStatistics = new PlayerGameStatistics();

                    playerGameStatistics.Game = game;
                    playerGameStatistics.Player = player;
                    playerGameStatistics.Points = player.Shots.Where(s => s.Game.Id == game.Id).Sum(s => s.Points);
                    playerGameStatistics.ShotsMade = player.Shots.Count(s => s.Game.Id == game.Id && s.ShotMade);
                    playerGameStatistics.Attempts = player.Shots.Where(s => s.Game.Id == game.Id).Sum(s => s.Attempts);
                    playerGameStatistics.ShootingPercentage = Decimal.Round((decimal)playerGameStatistics.ShotsMade / (decimal)playerGameStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
                    playerGameStatistics.Pushes = player.Shots.Count(s => s.Game.Id == game.Id && s.ShotType.Id == ShotTypePush);
                    playerGameStatistics.Steals = player.Shots.Count(s => s.Game.Id == game.Id && s.ShotType.Id == ShotTypeSteal);
                    playerGameStatistics.SugarFreeSteals = player.Shots.Count(s => s.Game.Id == game.Id && s.ShotType.Id == ShotTypeSugarFreeSteal);
                    playerGameStatistics.StainlessSteals = player.Shots.Count(s => s.Game.Id == game.Id && ((s.ShotType.Id == ShotTypeSteal || s.ShotType.Id == ShotTypeSteal) && s.Attempts == 1));
                    playerGameStatistics.GameWinningSteal = player.Shots.Any(s => s.Game.Id == game.Id && ((s.ShotType.Id == ShotTypeSteal || s.ShotType.Id == ShotTypeSteal) && s.Hole.Id == (game.Shots.Max(shot => shot.Hole.Id))));
                    playerGameStatistics.Winner = (playerGameStatistics.Points == maxPoints);
                    playerGameStatistics.OvertimeWin = player.Shots.Max(s => s.Hole.Id) > 10 && playerGameStatistics.Winner;

                    int totalGamePoints = game.Shots.Sum(s => s.Points);

                    playerGameStatistics.Shutout = playerGameStatistics.Points == totalGamePoints;
                    playerGameStatistics.PerfectGame = playerGameStatistics.Shutout && (playerGameStatistics.ShotsMade == playerGameStatistics.Attempts);

                    _playerGameStatisticsRepository.Add(playerGameStatistics);
                }

                var gameShots = game.Shots.ToList();

                // Calculate the total stats for the game
                var gameStatistics = new GameStatistics();

                gameStatistics.Game = game;
                gameStatistics.HoleCount = gameShots.Select(s => s.Hole.Id).Distinct().Count();
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
                gameStatistics.StainlessSteals = gameShots.Count(s => (s.ShotType.Id == ShotTypeSteal || s.ShotType.Id == ShotTypeSteal) && s.ShotMade && s.Attempts == 1);

                _gameStatisticsRepository.Add(gameStatistics);
            }

            _unitOfWork.Commit();
        }

        public void CalculateGameStatistics(int gameId)
        {
            Check.Argument.IsNotZeroOrNegative(gameId, "gameId");

            var game = _gameRepository.GetById(gameId);
            var players = game.Shots.Select(s => s.Player).Distinct();

            int maxPoints = 0;

            // The first thing that we want to calculate is the max number of points scored (the winner's points). This way 
            // we can easily determine who the winner is later when actually adding the stats to the repository.
            foreach (var player in players)
            {
                int playerPoints = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Points);

                if (playerPoints > maxPoints)
                {
                    maxPoints = playerPoints;
                }
            }

            // Calculate all of the game statistics for each player individually
            foreach (var player in players)
            {
                var playerGameStatistics = new PlayerGameStatistics();

                playerGameStatistics.Game = game;
                playerGameStatistics.Player = player;
                playerGameStatistics.Points = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Points);
                playerGameStatistics.ShotsMade = player.Shots.Count(s => s.Game.Id == gameId && s.ShotMade);
                playerGameStatistics.Attempts = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Attempts);
                playerGameStatistics.ShootingPercentage = Decimal.Round((decimal)playerGameStatistics.ShotsMade / (decimal)playerGameStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
                playerGameStatistics.Pushes = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypePush);
                playerGameStatistics.Steals = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypeSteal);
                playerGameStatistics.SugarFreeSteals = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypeSugarFreeSteal);
                playerGameStatistics.Winner = (playerGameStatistics.Points == maxPoints);

                _playerGameStatisticsRepository.Add(playerGameStatistics);
            }

            var gameShots = game.Shots.ToList();

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

            _unitOfWork.Commit();
        }
    }
}