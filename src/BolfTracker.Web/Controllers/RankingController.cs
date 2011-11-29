using System;
using System.Linq;
using System.Web.Mvc;

using BolfTracker.Services;

using MvcMiniProfiler;

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
            return RedirectToAction("Monthly", new { year = DateTime.Today.Year, month = DateTime.Today.Month });
        }

        public ActionResult Monthly(int? year, int? month)
        {
            int rankingsYear = year.HasValue ? year.Value : DateTime.Today.Year;
            int rankingsMonth = month.HasValue ? month.Value : DateTime.Today.Month;

            var rankings = _rankingService.GetRankings(rankingsMonth, rankingsYear);

            return View("Rankings", new RankingsViewModel(rankingsMonth, rankingsYear, rankings));
        }
    }
}
