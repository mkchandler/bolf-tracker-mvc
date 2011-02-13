using System;
using System.Linq;
using System.Web.Mvc;

using BolfTracker.Models;
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

            // Order the rankings based on our formula
            rankings = rankings.OrderBy(r => r.GamesBack)
                               .ThenByDescending(r => r.WinningPercentage)
                               .ThenByDescending(r => r.PointsPerGame)
                               .ThenByDescending(r => r.TotalPoints);

            return View("Rankings", rankings);
        }

        [HttpPost]
        public ActionResult Calculate()
        {
            _rankingService.CalculateRankings(DateTime.Today.Month, DateTime.Today.Year);

            return RedirectToAction("Index");
        }
    }
}