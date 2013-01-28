using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRankingService _rankingService;
        private readonly IPlayerService _playerService;

        public HomeController(IRankingService rankingService, IPlayerService playerService)
        {
            _rankingService = rankingService;
            _playerService = playerService;
        }

        public ActionResult Index()
        {
            int rankingsYear = DateTime.Today.Year;
            int rankingsMonth = DateTime.Today.Month;

            var rankings = _rankingService.GetRankings(rankingsMonth, rankingsYear);
            var playerStatistics = _playerService.GetPlayerStatistics(rankingsMonth, rankingsYear);

            return View("Home", new HomeViewModel(rankings, playerStatistics));
        }
    }
}
