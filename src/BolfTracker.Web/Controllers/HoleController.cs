using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
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

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
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