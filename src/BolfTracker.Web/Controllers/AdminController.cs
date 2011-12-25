using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BolfTracker.Services;
using MvcMiniProfiler;

namespace BolfTracker.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IHoleService _holeService;
        private readonly IPlayerService _playerService;
        private readonly IRankingService _rankingService;

        public AdminController(IGameService gameService, IHoleService holeService, IPlayerService playerService, IRankingService rankingService)
        {
            _gameService = gameService;
            _holeService = holeService;
            _playerService = playerService;
            _rankingService = rankingService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Players()
        {
            var allLeaguePlayers = _playerService.GetPlayers();

            return View(allLeaguePlayers);
        }

        [Authorize]
        public ActionResult Holes()
        {
            var allLeagueHoles = _holeService.GetHoles();

            return View(allLeagueHoles);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CalculateRankings(int month, int year)
        {
            if (month == 0 && year == 0)
            {
                //_rankingService.CalculateRankings();
            }
            else
            {
                _rankingService.CalculateRankings(month, year);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CalculateGameStatistics(int month, int year)
        {
            if (month == 0 && year == 0)
            {
                _gameService.CalculateGameStatistics();
            }
            else
            {
                _gameService.CalculateGameStatistics(month, year);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CalculatePlayerStatistics(int month, int year)
        {
            var profiler = MiniProfiler.Current;

            using (profiler.Step("Calculate player statistics"))
            {
                if (month == 0 && year == 0)
                {
                    _playerService.CalculatePlayerStatistics();
                }
                else
                {
                    _playerService.CalculatePlayerStatistics(month, year);
                }
            }

            using (profiler.Step("Calculate player hole statistics"))
            {
                if (month == 0 && year == 0)
                {
                    _playerService.CalculatePlayerHoleStatistics();
                }
                else
                {
                    _playerService.CalculatePlayerHoleStatistics(month, year);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CalculateHoleStatistics(int month, int year)
        {
            if (month == 0 && year == 0)
            {
                //_holeService.CalculateHoleStatistics();
            }
            else
            {
                _holeService.CalculateHoleStatistics(month, year);
            }

            return RedirectToAction("Index");
        }
    }
}
