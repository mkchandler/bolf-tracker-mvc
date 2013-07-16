using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class HoleController : Controller
    {
        private readonly IHoleService _holeService;

        public HoleController(IHoleService holeService)
        {
            _holeService = holeService;
        }

        public ActionResult Index()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            var holesStatistics = _holeService.GetHoleStatistics(month, year);
            var lifetimeHoleStatistics = _holeService.GetHoleStatistics();

            return View("Holes", new HolesViewModel(month, year, holesStatistics, lifetimeHoleStatistics));
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        [Authorize]
        public ActionResult Create(int holeNumber, int par)
        {
            try
            {
                _holeService.CreateHole(holeNumber, par);

                return RedirectToAction("Holes", "Admin");
            }
            catch
            {
                return View();
            }
        }
    }
}
