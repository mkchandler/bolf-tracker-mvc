using System;
using System.Web.Mvc;

using BolfTracker.Models;
using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IHoleService _holeService;
        private readonly IPlayerService _playerService;
        private readonly IShotTypeService _scoreTypeService;

        public GameController(IGameService gameService, IHoleService holeService, IPlayerService playerService, IShotTypeService scoreTypeService)
        {
            _gameService = gameService;
            _holeService = holeService;
            _playerService = playerService;
            _scoreTypeService = scoreTypeService;
        }

        public ActionResult Index()
        {
            var games = _gameService.GetGames(DateTime.Today.Month, DateTime.Today.Year);

            return View(games);
        }

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

        public ActionResult Create()
        {
            return View(new Game());
        }

        [HttpPost]
        public ActionResult Create(DateTime date)
        {
            try
            {
                var game = _gameService.CreateGame(date);

                return RedirectToAction("Details", new { id = game.Id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Finalize(int gameId)
        {
            _gameService.CalculateGameStatistics(gameId);

            return RedirectToAction("Details", new { id = gameId });
        }
    }
}