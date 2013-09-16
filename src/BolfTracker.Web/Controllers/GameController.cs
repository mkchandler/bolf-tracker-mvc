﻿using System;
using System.Web.Mvc;

using BolfTracker.Models;
using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IShotService _shotService;
        private readonly IHoleService _holeService;
        private readonly IPlayerService _playerService;
        private readonly IRankingService _rankingService;
        private readonly IShotTypeService _scoreTypeService;

        public GameController(IGameService gameService, IShotService shotService, IHoleService holeService, IPlayerService playerService, IRankingService rankingService, IShotTypeService scoreTypeService)
        {
            _gameService = gameService;
            _shotService = shotService;
            _holeService = holeService;
            _playerService = playerService;
            _rankingService = rankingService;
            _scoreTypeService = scoreTypeService;
        }

        public ActionResult Index(int? year, int? month)
        {
            int gamesYear = year.HasValue ? year.Value : DateTime.Today.Year;
            int gamesMonth = month.HasValue ? month.Value : DateTime.Today.Month;

            var games = _gameService.GetGames(gamesMonth, gamesYear);
            var playerGameStatistics = _playerService.GetPlayerGameStatistics(gamesMonth, gamesYear);

            return View(new GamesViewModel(gamesMonth, gamesYear, games, playerGameStatistics));
        }

        [Authorize]
        public ActionResult RecalculateGameStatistics()
        {
            _gameService.CalculateGameStatistics();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var game = _gameService.GetGame(id);
            var shots = _shotService.GetShots(id);
            var allPlayers = _playerService.GetPlayers();
            var allHoles = _holeService.GetHoles();

            var gamePanel = new GamePanelViewModel(game, shots, allPlayers, allHoles);

            return View(gamePanel);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new Game());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(DateTime date)
        {
            var game = _gameService.CreateGame(date);

            return RedirectToAction("Details", new { id = game.Id });
        }

        public ActionResult Delete(int gameId)
        {
            _gameService.DeleteGame(gameId);

            return RedirectToAction("Index", new { year = DateTime.Today.Year, month = DateTime.Today.Month });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Finalize(int gameId)
        {
            var game = _gameService.GetGame(gameId);

            _gameService.CalculateGameStatistics(gameId);
            _gameService.CalculatePlayerRivalryStatistics(gameId);
            _playerService.CalculatePlayerStatistics(game.Date.Month, game.Date.Year, true);
            _holeService.CalculateHoleStatistics(game.Date.Month, game.Date.Year);
            _rankingService.CalculateRankings(game.Date.Month, game.Date.Year);

            return RedirectToAction("Details", new { id = gameId });
        }

        [HttpPost]
        [Authorize]
        public ActionResult UnFinalize(int gameId)
        {
            var game = _gameService.GetGame(gameId);
            
            // Delete game and player game statistics
            _gameService.DeleteGameStatistics(gameId);

            // Delete player rivalry statistics for the game
            _gameService.DeletePlayerRivalryStatistics(gameId);

            // Recalculate the stats for the month (and career) without taking into account this game
            _playerService.CalculatePlayerStatistics(game.Date.Month, game.Date.Year, true);
            _holeService.CalculateHoleStatistics(game.Date.Month, game.Date.Year);
            _rankingService.CalculateRankings(game.Date.Month, game.Date.Year);

            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
