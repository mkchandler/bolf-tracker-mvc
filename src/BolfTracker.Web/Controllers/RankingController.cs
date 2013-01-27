using System;
using System.Linq;
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

        public ActionResult Index(int? year, int? month)
        {
            int rankingsYear = year.HasValue ? year.Value : DateTime.Today.Year;
            int rankingsMonth = month.HasValue ? month.Value : DateTime.Today.Month;

            int eligibilityLine = _rankingService.GetEligibilityLine(rankingsMonth, rankingsYear);
            var rankings = _rankingService.GetRankings(rankingsMonth, rankingsYear);

            return View("Rankings", new RankingsViewModel(rankingsMonth, rankingsYear, eligibilityLine, rankings));
        }
    }
}
