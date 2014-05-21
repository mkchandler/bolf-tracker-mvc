using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    [Authorize]
    public class ShotController : Controller
    {
        private readonly IShotService _shotService;

        public ShotController(IShotService scoreService)
        {
            _shotService = scoreService;
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            int gameId = Convert.ToInt32(collection["Game.Id"]);
            int playerId = Convert.ToInt32(collection["Player"]);
            int holeId = Convert.ToInt32(collection["Hole"]);
            int attempts = Convert.ToInt32(collection["attempts"]);
            bool shotMade = collection["shotMade"] == "on" ? true : false;

            _shotService.Create(gameId, playerId, holeId, attempts, shotMade);

            return RedirectToAction("Details", "Game", new { id = gameId });
        }

        [HttpPost]
        public ActionResult Delete(int scoreId, int gameId)
        {
            _shotService.Delete(scoreId);

            return RedirectToAction("Details", "Game", new { id = gameId });
        }

        [HttpPost]
        public ActionResult DeleteToShot(int gameId, int shotId)
        {
            _shotService.DeleteToShot(gameId, shotId);

            return RedirectToAction("Details", "Game", new { id = gameId });
        }
    }
}
