using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRankingService _rankingService;

        public HomeController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        public ActionResult Index()
        {
            int rankingsYear = DateTime.Today.Year;
            int rankingsMonth = DateTime.Today.Month;

            int eligibilityLine = _rankingService.GetEligibilityLine(rankingsMonth, rankingsYear);
            var rankings = _rankingService.GetRankings(rankingsMonth, rankingsYear);

            return View("Rankings", new RankingsViewModel(rankingsMonth, rankingsYear, eligibilityLine, rankings));
        }
    }
}
