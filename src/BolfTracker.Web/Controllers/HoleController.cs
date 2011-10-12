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

            var holes = _holeService.GetHoles();
            var holesStatistics = _holeService.GetHoleStatistics(month, year);

            return View("Holes", new HolesViewModel(holes, holesStatistics));
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
