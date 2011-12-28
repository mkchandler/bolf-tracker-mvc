using System;
using System.Web.Mvc;

using BolfTracker.Models;
using BolfTracker.Services;

using MvcMiniProfiler;

namespace BolfTracker.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IHoleService _holeService;
        private readonly IPlayerService _playerService;
        private readonly IRankingService _rankingService;
        private readonly IShotTypeService _scoreTypeService;

        public GameController(IGameService gameService, IHoleService holeService, IPlayerService playerService, IRankingService rankingService, IShotTypeService scoreTypeService)
        {
            _gameService = gameService;
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
            var allPlayers = _playerService.GetPlayers();
            var allHoles = _holeService.GetHoles();

            var gamePanel = new GamePanelViewModel(game, allPlayers, allHoles);

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

        [HttpPost]
        [Authorize]
        public ActionResult Finalize(int gameId)
        {
            var profiler = MiniProfiler.Current;

            var game = _gameService.GetGame(gameId);

            using (profiler.Step("Calculate game statistics"))
            {
                _gameService.CalculateGameStatistics(gameId);
            }

            using (profiler.Step("Calculate rankings"))
            {
                _rankingService.CalculateRankings(game.Date.Month, game.Date.Year);
            }

            using (profiler.Step("Calculate player statistics"))
            {
                _playerService.CalculatePlayerStatistics(game.Date.Month, game.Date.Year, true);
            }

            using (profiler.Step("Calculate hole statistics"))
            {
                _holeService.CalculateHoleStatistics(game.Date.Month, game.Date.Year);
            }

            return RedirectToAction("Details", new { id = gameId });
        }
    }
}
