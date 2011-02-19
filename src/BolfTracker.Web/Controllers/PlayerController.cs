using System;
using System.Web.Mvc;

using BolfTracker.Services;

namespace BolfTracker.Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public ActionResult Index()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            var players = _playerService.GetPlayers();
            var playerStatistics = _playerService.GetPlayerStatistics(month, year);

            return View(new PlayersViewModel(players, playerStatistics));
        }

        public ActionResult Details(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(player);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(string name)
        {
            try
            {
                _playerService.Create(name);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
 
        public ActionResult Edit(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(player);
        }

        [HttpPost]
        public ActionResult Edit(int id, string name)
        {
            try
            {
                _playerService.Update(id, name);
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(player);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection formCollection)
        {
            try
            {
                _playerService.Delete(id);
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}