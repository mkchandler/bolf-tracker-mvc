using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class RankingController : Controller
    {
        private readonly IRankingService _rankingService;
        private readonly IPlayerService _playerService;
        private readonly IHoleService _holeService;

        public RankingController(IRankingService rankingService, IPlayerService playerService, IHoleService holeService)
        {
            _rankingService = rankingService;
            _playerService = playerService;
            _holeService = holeService;
        }

        public ActionResult Index()
        {
            var rankings = _rankingService.GetRankings(DateTime.Today.Month, DateTime.Today.Year);

            return View("Rankings", new RankingsViewModel(rankings));
        }

        [HttpPost]
        public ActionResult Calculate()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            _rankingService.CalculateRankings(month, year);

            _playerService.CalculatePlayerStatistics(month, year);

            _playerService.CalculatePlayerHoleStatistics(month, year);

            _holeService.CalculateHoleStatistics(month, year);

            return RedirectToAction("Index");
        }
    }
}