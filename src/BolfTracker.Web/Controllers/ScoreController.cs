using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class ScoreController : Controller
    {
        private readonly IShotService _shotService;

        public ScoreController(IShotService scoreService)
        {
            _shotService = scoreService;
        }

        public ActionResult Index()
        {
            return View();
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
                int gameId = Convert.ToInt32(collection["Game.Id"]);
                int playerId = Convert.ToInt32(collection["Player"]);
                int holeId = Convert.ToInt32(collection["Hole"]);
                int attempts = Convert.ToInt32(collection["attempts"]);
                bool shotMade = collection["shotMade"] == "on" ? true : false;

                _shotService.Create(gameId, playerId, holeId, attempts, shotMade);

                return RedirectToAction("Details", "Game", new { id = gameId });
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

        [HttpPost]
        public ActionResult Delete(int scoreId, int gameId)
        {
            try
            {
                _shotService.Delete(scoreId);

                return RedirectToAction("Details", "Game", new { id = gameId });
            }
            catch
            {
                return View();
            }
        }
    }
}
