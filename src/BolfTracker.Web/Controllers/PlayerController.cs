using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;

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

        [Authorize]
        public ActionResult Index()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            var players = _playerService.GetPlayers();
            var playerStatistics = _playerService.GetPlayerStatistics(month, year);

            return View(new PlayersViewModel(players, playerStatistics));
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(new PlayerViewModel(player));
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
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

        [Authorize]
        public ActionResult Edit(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(player);
        }

        [Authorize]
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

        [Authorize]
        public ActionResult Delete(int id)
        {
            var player = _playerService.GetPlayer(id);

            return View(player);
        }

        [Authorize]
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticate(string password)
        {
            var privateBetaCode = ConfigurationManager.AppSettings["PrivateBetaCode"];

            if (password == privateBetaCode)
            {
                FormsAuthentication.SetAuthCookie("PrivateBetaUser", false);

                return RedirectToAction("Index", "Ranking");
            }

            return RedirectToAction("Login");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}