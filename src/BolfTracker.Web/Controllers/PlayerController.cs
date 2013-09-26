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
            //var stats = new List<PlayerCareerStatistics>();
            //stats.Add(new PlayerCareerStatistics
            //{
            //    Player = new Player { Name = "Eh-Hole" },
            //    Wins = 325,
            //    Losses = 456,
            //    ShotsMade = 4212,
            //    Attempts = 13740,
            //    ShootingPercentage = (decimal)0.307,
            //    Points = 8124,
            //    PointsPerGame = (decimal)10.4,
            //    Pushes = 1224,
            //    Steals = 291,
            //    SugarFreeSteals = 57
            //});

            //stats.Add(new PlayerCareerStatistics
            //{
            //    Player = new Player { Name = "Furia Rojo" },
            //    Wins = 208,
            //    Losses = 484,
            //    ShotsMade = 3339,
            //    Attempts = 12369,
            //    ShootingPercentage = (decimal)0.27,
            //    Points = 5713,
            //    PointsPerGame = (decimal)8.3,
            //    Pushes = 944,
            //    Steals = 221,
            //    SugarFreeSteals = 40
            //});

            //stats.Add(new PlayerCareerStatistics
            //{
            //    Player = new Player { Name = "I Know Bitnezz" },
            //    Wins = 171,
            //    Losses = 625,
            //    ShotsMade = 3052,
            //    Attempts = 14449,
            //    ShootingPercentage = (decimal)0.21,
            //    Points = 4889,
            //    PointsPerGame = (decimal)6.1,
            //    Pushes = 871,
            //    Steals = 160,
            //    SugarFreeSteals = 30
            //});

            //var json =
            //    stats.Select(
            //        a =>
            //        new
            //        {
            //            a.Player.Name,
            //            a.ShotsMade,
            //            a.ShotsMissed,
            //            a.ShootingPercentage,
            //            a.Points,
            //            a.PointsPerGame,
            //            a.Wins,
            //            a.Pushes,
            //            a.Steals,
            //            a.SugarFreeSteals
            //        });
            //return Json(json, JsonRequestBehavior.AllowGet);
            return Json( _playerService.GetPlayerCareerStatistics()
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
