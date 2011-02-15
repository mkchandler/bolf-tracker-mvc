using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class RankingController : Controller
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        public ActionResult Index()
        {
            var rankings = _rankingService.GetRankings(DateTime.Today.Month, DateTime.Today.Year);

            return View("Rankings", new RankingsViewModel(rankings));
        }

        [HttpPost]
        public ActionResult Calculate()
        {
            _rankingService.CalculateRankings(DateTime.Today.Month, DateTime.Today.Year);

            return RedirectToAction("Index");
        }
    }
}