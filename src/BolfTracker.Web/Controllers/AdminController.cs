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

            return View("Index");
        }
    }
}
