using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BolfTracker.Web.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
