using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BolfTracker.Models;
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

            var playerStatistics = _playerService.GetPlayerStatistics(month, year);
            var playerCareerStatistics = _playerService.GetPlayerCareerStatistics();

            return View(new PlayersViewModel(month, year, playerStatistics, playerCareerStatistics));
        }

        public JsonResult GetPlayerStatisticsJson()
        {
            return Json( _playerService.GetPlayerCareerStatistics()
                .OrderBy(a=>a.Player.Name)
                .Select(
                    a =>
                    new
                    {
                        a.Player.Name,
                        a.ShotsMade,
                        a.ShotsMissed,
                        a.ShootingPercentage,
                        a.Points,
                        a.PointsPerGame,
                        a.Wins,
                        a.Pushes,
                        a.Steals,
                        a.SugarFreeSteals
                    }), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int id, string name)
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            var player = _playerService.GetPlayer(id);
            var playerStatistics = _playerService.GetPlayerStatistics(id);
            var playerCareerStatistics = _playerService.GetPlayerCareerStatistics(id);
            var playerHoleStatistics = _playerService.GetPlayerHoleStatistics(id, month, year);

            return View(new PlayerViewModel(month, year, player, playerStatistics, playerCareerStatistics, playerHoleStatistics));
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(string name)
        {
            try
            {
                _playerService.Create(name);

                return RedirectToAction("Players", "Admin");
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

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, string name)
        {
            try
            {
                _playerService.Update(id, name);

                return RedirectToAction("Players", "Admin");
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
