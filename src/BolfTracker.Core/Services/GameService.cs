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
        private readonly IUnitOfWork _unitOfWork;

        // TODO: Really need to figure out a better way to do this
        private const int ShotTypePush = 3;
        private const int ShotTypeSteal = 4;
        private const int ShotTypeSugarFreeSteal = 5;

        public GameService(IGameRepository gameRepository, IGameStatisticsRepository gameStatisticsRepository, IUnitOfWork unitOfWork)
        {
            _gameRepository = gameRepository;
            _gameStatisticsRepository = gameStatisticsRepository;
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

            foreach (var player in players)
            {
                var gameStatistics = new GameStatistics();

                gameStatistics.Game = game;
                gameStatistics.Player = player;
                gameStatistics.Points = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Points);
                gameStatistics.ShotsMade = player.Shots.Count(s => s.Game.Id == gameId && s.ShotMade);
                gameStatistics.Attempts = player.Shots.Where(s => s.Game.Id == gameId).Sum(s => s.Attempts);
                gameStatistics.Pushes = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypePush);
                gameStatistics.Steals = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypeSteal);
                gameStatistics.SugarFreeSteals = player.Shots.Count(s => s.Game.Id == gameId && s.ShotType.Id == ShotTypeSugarFreeSteal);
                gameStatistics.Winner = (gameStatistics.Points == maxPoints);

                _gameStatisticsRepository.Add(gameStatistics);
            }

            _unitOfWork.Commit();
        }
    }
}